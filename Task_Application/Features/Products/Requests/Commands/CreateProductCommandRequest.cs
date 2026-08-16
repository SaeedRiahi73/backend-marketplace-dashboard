using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Product;

namespace Task_Application.Features.Products.Requests.Commands
{
    public class CreateProductCommandRequest:IRequest<ResultInfo<Guid>>
    {
        public CreateProductDto CreateProduct { get; set; }
    }
}
