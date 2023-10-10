using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IReviewerRepository reviewerRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IMapper mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.reviewerRepository = reviewerRepository;
            this.pokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await reviewRepository.GetReviews();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<ReviewDto>>(reviews));
        }

        [HttpGet]
        [Route("{reviewId:int}")]
        public async Task<IActionResult> GetReview([FromRoute]int reviewId)
        {
            if(!await reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = await reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<ReviewDto>(review));
        }

        [HttpGet]
        [Route("/{pokeId:int}/reviews")]
        public async Task<IActionResult> GetReviewsOfAPokemon([FromRoute] int pokeId)
        {
            var reviews = await reviewRepository.GetReviewsOfAPokemon(pokeId); 
            if(!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<ReviewDto>>(reviews));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, ReviewDto review)
        {
            if (review == null)
            {
                return BadRequest(ModelState);
            }
            var existingReview = await reviewRepository.HasReview(review.Title);
            if (existingReview != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = mapper.Map<Review>(review);
            reviewMap.Reviewer = await reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = await pokemonRepository.GetPokemon(pokeId);
            if (!await reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
