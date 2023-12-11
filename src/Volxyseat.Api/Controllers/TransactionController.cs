using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.SubscriptionModel;
using Volxyseat.Domain.Models.Transaction;
using Volxyseat.Domain.ViewModel;
using Volxyseat.Infrastructure.Repository;

namespace Volxyseat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepository, IUnitOfWork uow, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allSubscriptions = await _transactionRepository.GetAll();

                if (allSubscriptions == null)
                {
                    return NotFound();
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
                var existingTransaction = await _transactionRepository.GetById(Id);

                if(existingTransaction == null)
                {
                    return BadRequest("Not Found");
                }

                return Ok(existingTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TransactionViewModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("O objeto de solicitação é nulo.");
                }

                var map = _mapper.Map<TransactionModel>(request);
                _transactionRepository.Add(map);
                await _uow.SaveChangesAsync();

                return Ok(map);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

    }
}
