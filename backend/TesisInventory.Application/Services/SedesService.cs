using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class SedesService : ISedesService
    {
        private readonly ISedeRepository _sedeRepository;

        public SedesService(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public async Task<IEnumerable<Sede>> GetAllSedesAsync()
        {
            return await _sedeRepository.GetAllAsync();
        }

        public async Task<Sede?> GetSedeByIdAsync(int id)
        {
            return await _sedeRepository.GetByIdAsync(id);
        }
    }
}
