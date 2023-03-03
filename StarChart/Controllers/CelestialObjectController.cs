using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
	[Route("")]
	[ApiController]
	public class CelestialObjectController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public CelestialObjectController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("{id:int}",Name ="GetById")]
		public IActionResult GetById(int id)
		{
			var celestialObject = _context.celestialObjects.Find(id);
			if(celestialObject == null)
			{
				return NotFound();
			}
			celestialObject.Satellites = _context.celestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
			return Ok(celestialObject);
		}

		[HttpGet("{name}")]
		public IActionResult GetByName(string name)
		{
			var celestialObjects = _context.celestialObjects.Where(_e => _e.Name == name).ToList();
			if (!celestialObjects.Any())
				return NotFound();
			foreach(var celestialObject in celestialObjects)
			{
				celestialObject.Satellites = _context.celestialObjects.Where(e => e.OrbitedObjectId == celestialObject.id).ToList();
			}
			return Ok(celestialObjects);
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var celestialObjects = _context.celestialObjects.ToList();
			foreach(var celestialObject in celestialObjects)
			{
				celestialObject.Satellites = _context.celestialObjects.Where(e=>e.OrbitedObjectId==celestialObject.id).ToList();
			}
			return Ok(celestialObjects);
		}
	}
}
