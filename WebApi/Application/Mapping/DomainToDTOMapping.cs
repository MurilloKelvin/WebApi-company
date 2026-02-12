using AutoMapper;
using WebApi.Application.ViewModel;
using WebApi.Domain.Models.EmployeesAggregate;
using static WebApi.Domain.DTO.EmployeeDTO;

namespace WebApi.Application.Mapping

{
    public class DomainToDTOMapping : Profile
    {
        // antigo ToView
        public DomainToDTOMapping()
        {
            CreateMap<Employee, EmployeeResponseDTO>(); // Mapeia a entidade Employee para o DTO EmployeeResponseDTO cliente
            CreateMap<EmployeeViewModel, Employee>(); // Mapeia o ViewModel EmployeeViewModel para a entidade Employee, permitindo a conversão dos dados recebidos do cliente para o formato esperado pelo banco de dados

        }
    }
}
