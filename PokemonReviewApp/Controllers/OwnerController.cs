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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            this.ownerRepository = ownerRepository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetOwners()
        {
            var owners = await ownerRepository.GetOwners();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<OwnerDto>>(owners));
        }

        [HttpGet]
        [Route("{ownerId:int}")]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            if(! await ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = await ownerRepository.GetOwner(ownerId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<OwnerDto>(owner));
        }
        [HttpGet]
        [Route("/{pokeId}/owners")]
        public async Task<IActionResult> GetOwnersOfAPokemon(int pokeId)
        {
            var owners = await ownerRepository.GetOwnersOfAPokemon(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<OwnerDto>>(owners));
        }
        [HttpGet]
        [Route("{ownerId}/pokemons")]
        public async Task<IActionResult> GetPokemonByOwner(int ownerId)
        {
            if (!await ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var pokemon = await ownerRepository.GetPokemonByOwner(ownerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<PokemonDto>>(pokemon));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner(OwnerDto owner, [FromQuery]int countryId)
        {
            if(owner == null)
            {
                return BadRequest(ModelState);
            }
            var existingOwner = await ownerRepository.HasOwner(owner.FirstName, owner.LastName);
            if(existingOwner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = mapper.Map<Owner>(owner);
            ownerMap.Country = await countryRepository.GetCountry(countryId);
            if(! await ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
