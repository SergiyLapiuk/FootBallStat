using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootBallStat;

namespace FootBallStat.Controllers
{
    public class MatchesController : Controller
    {
        private readonly DBFootballStatContext _context;

        public MatchesController(DBFootballStatContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var dBFootballStatContext = _context.Matches.Include(m => m.Championship).Include(m => m.Team1).Include(m => m.Team2);
            return View(await dBFootballStatContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            //return View(match);
            return RedirectToAction("Index", "PlayersInMatches", new { id = match.Id, team1Id = match.Team1Id, team2Id = match.Team2Id,
                championshipId = match.ChampionshipId, date = match.Date });
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Name");
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Name");
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Team1Id,Team2Id,ChampionshipId,Date")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Id", match.ChampionshipId);
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Id", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Id", match.Team2Id);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            } /////
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Name", match.ChampionshipId); 
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Name", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Name", match.Team2Id);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Team1Id,Team2Id,ChampionshipId,Date")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
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
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Id", match.ChampionshipId);
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Id", match.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Id", match.Team2Id);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
