using AutoMapper;
using WebApi.Application.ViewModel;
using WebApi.Domain.DTO;
using WebApi.Domain.Models;
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
            CreateMap<Employee, EmployeeSummaryDTO>(); // Mapeia a entidade Employee para o DTO EmployeeSumaryDTO, permitindo a conversão dos dados da entidade para um formato mais resumido e adequado para exibição em listas ou resumos
            CreateMap<EmployeeViewModel, Employee>(); // Mapeia o ViewModel EmployeeViewModel para a entidade Employee, permitindo a conversão dos dados recebidos do cliente para o formato esperado pelo banco de dados
            CreateMap<UserViewModel, User>(); // Mapeia o ViewModel UserViewModel para a entidade User, permitindo a conversão dos dados recebidos do cliente para o formato esperado pelo banco de dados
            CreateMap<User, UserViewModel>(); //         
            CreateMap<User, UserResponseDTO>();
        }
    }
}
