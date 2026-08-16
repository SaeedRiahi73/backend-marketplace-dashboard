using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;

namespace Task_Application.Features.Products.Requests.Commands
{
    public class DeleteProductCommandRequest:IRequest<ResultInfo<Unit>>
    {
        public Guid productId { get; set; }
    }
}
