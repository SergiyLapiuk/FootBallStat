using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootBallStat;
using ClosedXML.Excel;

namespace FootBallStat.Controllers
{
    public class TeamsController : Controller
    {
        private readonly DBFootballStatContext _context;

        public TeamsController(DBFootballStatContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            //return View(team);
            return RedirectToAction("Index", "Players", new { id = team.Id, name = team.Name });
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Team team)
        {
            if (IsUnique(team.Name)) 
            {
                if (ModelState.IsValid)
                {
                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така команда вже існує!";
            }
            return View(team);
        }

        bool IsUnique(string name)
        {
            var q = (from team in _context.Teams
                     where team.Name == name
                     select team).ToList();
            if (q.Count == 0) { return true; }
            return false;
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (IsUnique(team.Name)) 
            { 
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(team);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TeamExists(team.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така команда вже існує!";
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            int count_players = _context.Players.Where(m => m.TeamId == id).Count();
            int count_matches = _context.Matches.Where(m => m.Team1Id == id || m.Team2Id == id).Count();
            if (count_players != 0)
            {
                ViewData["ErrorMessage"] = "Видалення не можливе, бо в команді є гравці!";
                return View(team);
            }
            else if(count_matches != 0) 
            {
                ViewData["ErrorMessage"] = "Видалення не можливе, бо команда бере участь в матчах!";
                return View(team);
            }
            else
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Team newteam;
                                var c = (from team in _context.Teams
                                         where team.Name.Contains(worksheet.Name)
                                         select team).ToList();
                                if (c.Count > 0)
                                {
                                    newteam = c[0];
                                }
                                else
                                {
                                    newteam = new Team();
                                    newteam.Name = worksheet.Name;
                                    _context.Teams.Add(newteam);
                                }
                                var plfile = new List<Player>();
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        if(row.Cell(4).Value.ToString().Length > 0)
                                        {
                                            Position position;
                                            var p = (from pos in _context.Positions
                                                     where pos.Name.Contains(row.Cell(4).Value.ToString())
                                                     select pos).ToList();
                                            if(p.Count > 0)
                                            {
                                                position = p[0];
                                            }
                                            else
                                            {
                                                position = new Position();
                                                position.Name = row.Cell(4).Value.ToString();
                                                _context.Positions.Add(position);
                                            }
                                            Player player = new Player();
                                            player.Name = row.Cell(1).Value.ToString();
                                            var dateString = row.Cell(2).Value.ToString();
                                            DateTime date = DateTime.Parse(dateString);
                                            player.DateOfBirth = date;
                                            player.Number = Convert.ToInt32(row.Cell(3).Value.ToString());
                                            player.Team = newteam;
                                            player.Position = position;
                                            plfile.Add(player);
                                            if (IsUniquePlayer(player.Name, player.Number, newteam.Id, player.DateOfBirth) && (player.Number <= 99 && player.Number >= 1)
                                                && IsValidDate(player.DateOfBirth))
                                            { 
                                                _context.Players.Add(player);
                                                await _context.SaveChangesAsync();
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        bool IsUniquePlayer(string name, int number, int teamId, DateTime dateOfBirth)
        {
            var q = (from player in _context.Players
                     where player.Name == name && player.DateOfBirth.Date == dateOfBirth.Date
                     select player).ToList();

            var g = (from player in _context.Players
                         where player.Number == number && player.TeamId == teamId
                     
                     select player).ToList();
            if (q.Count == 0 && g.Count == 0) return true;
            return false;
        }

        //bool IsUniquePlayerFile(List<Player> players, string name, int number, int teamId, DateTime dateOfBirth)
        //{
        //    var q = (from player in players
        //             where player.Name == name && player.DateOfBirth.Date == dateOfBirth.Date
        //             select player).ToList();

        //    var g = (from player in players
        //             where player.Number == number
        //             select player).ToList();
        //    if (q.Count == 0 && g.Count == 0) return true;
        //    return false;
        //}

        bool IsValidDate(DateTime dateTime)
        {
            var dateString = "1/1/1977 0:00:00 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture);
            var dateString2 = "1/1/2005 0:00:00 AM";
            DateTime date2 = DateTime.Parse(dateString2,
                                      System.Globalization.CultureInfo.InvariantCulture);
            return date1 <= dateTime && dateTime <= date2;
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var teams = _context.Teams.Include("Players").ToList();
                foreach (var c in teams)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name);
                    worksheet.Cells("A1").Value = "Ім'я";
                    worksheet.Column("A").Width = 25;
                    worksheet.Row(1).Style.Font.Bold = true;

                    worksheet.Cells("B1").Value = "Дата народження";
                    worksheet.Column("B").Width = 25;
                    worksheet.Row(1).Style.Font.Bold = true;

                    worksheet.Cells("C1").Value = "Ігровий номер";
                    worksheet.Column("C").Width = 25;
                    worksheet.Row(1).Style.Font.Bold = true;

                    worksheet.Cells("D1").Value = "Позиція";
                    worksheet.Column("D").Width = 25;
                    worksheet.Row(1).Style.Font.Bold = true;

                    var players = c.Players.ToList();

                    for (int i = 0; i < players.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = players[i].Name;
                        worksheet.Cell(i + 2, 2).Value = players[i].DateOfBirth;
                        worksheet.Cell(i + 2, 3).Value = players[i].Number;

                        var p = _context.Positions.Where(a => a.Id == players[i].PositionId).FirstOrDefault();

                        worksheet.Cell(i + 2, 4).Value = p.Name;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"footboll_teams_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
