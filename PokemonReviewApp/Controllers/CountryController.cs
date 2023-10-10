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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await countryRepository.GetCountries();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<CountryDto>>(countries));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCountry([FromRoute] int id)
        {
            if (!await countryRepository.CountryExists(id))
            {
                return NotFound();
            }

            var country = await countryRepository.GetCountry(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mapper.Map<CountryDto>(country));
        }

        [HttpGet]
        [Route("/owners/{ownerId}/country")]
        public async Task<IActionResult> GetCountryByOwner([FromRoute]int ownerId)
        {
            var country = await countryRepository.GetCountryByOwner(ownerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<CountryDto>(country));
        }

        [HttpGet]
        [Route("{countryId}/owners")]
        public async Task<IActionResult> GetOwnersFromACountry([FromRoute] int countryId)
        {
            if(! await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var owners = await countryRepository.GetOwnersFromACountry(countryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody]CountryDto country)
        {
            if(country == null)
            {
                return BadRequest(ModelState);
            }

            var existingCountry = await countryRepository.HasCountry(country.Name);

            if (existingCountry != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = mapper.Map<Country>(country);
            if(! await countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
