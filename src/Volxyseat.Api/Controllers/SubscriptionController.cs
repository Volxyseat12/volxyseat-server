using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.SubscriptionModel;
using Volxyseat.Domain.ViewModel;
using Volxyseat.Infrastructure.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Volxyseat.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Volxyseat.Domain.Models.Transaction;

namespace Volxyseat.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository, IUnitOfWork uow, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allSubscriptions = await _subscriptionRepository.GetAll();

                if (allSubscriptions == null)
                {
                    return NotFound("Planos não encontrados");
                }

                var activeSubscriptions = allSubscriptions.Where(subscription => subscription.IsActive == true).ToList();

                if (activeSubscriptions.Count == 0)
                {
                    return NotFound("Nenhum plano ativo encontrado");
                }

                return Ok(allSubscriptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var subscriptionClient = await _subscriptionRepository.GetById(Id);
                if (subscriptionClient == null)
                {
                    return NotFound("Esse plano não foi encontrado");
                }

                return Ok(subscriptionClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] SubscriptionViewModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("O objeto de solicitação é nulo.");
                }

                var map = _mapper.Map<Subscription>(request);
                _subscriptionRepository.Add(map);
                await _uow.SaveChangesAsync();

                var responseViewModel = _mapper.Map<SubscriptionViewModel>(map);
                return Ok(responseViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody]SubscriptionViewModel request )
        {
            try
            {
                var existingSubscription = await _subscriptionRepository.GetById(request.Id);

                if (request.Id != existingSubscription.Id)
                {
                    return BadRequest("O ID da solicitação não corresponde ao ID existente.");
                }

                var map = _mapper.Map<Subscription>(request);

                _subscriptionRepository.Update(map);
                await _uow.SaveChangesAsync();

                return Ok(existingSubscription);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> switchSubscription(Guid Id)
        {
            try
            {
                var existingSubscription = await _subscriptionRepository.GetById(Id);
                if (Id != existingSubscription.Id)
                {
                    return BadRequest("O ID da solicitação não corresponde ao ID existente.");
                }

                existingSubscription.IsActive = !existingSubscription.IsActive;

                _subscriptionRepository.SwitchSubscription(existingSubscription);
                await _uow.SaveChangesAsync();

                return Ok(existingSubscription);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
