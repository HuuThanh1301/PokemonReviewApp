using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository pokemonRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            this.pokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemon = await pokemonRepository.GetPokemons();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<PokemonDto>>(pokemon));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPokemon([FromRoute] int id)
        {
            if (!pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            var pokemon = await pokemonRepository.GetPokemon(id);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<PokemonDto>(pokemon));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetPokemon([FromRoute] string name)
        {

            var pokemon = await pokemonRepository.GetPokemon(name);
            if (pokemon == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<PokemonDto>(pokemon));
        }

        [HttpGet]
        [Route("{id}/rating")]
        public async Task<IActionResult> GetPokemonRating([FromRoute] int id)
        {
            if (!pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            var rating = await pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, PokemonDto pokemon)
        {
            if(pokemon == null)
            {
                return BadRequest(ModelState);
            }

            var existingPokemon = await pokemonRepository.HasPokemon(pokemon.Name);
            if (existingPokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = mapper.Map<Pokemon>(pokemon);
            if(! await pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
