using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class GetLastActivityCreated
    {
        public class Query : IRequest<Result<int>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users
                                .Include(x => x.Activities)
                                .FirstOrDefaultAsync(x => x.UserName == request.Username);

                int idLastActivity = user.Activities.Max(a => a.ActivityId);

                return Result<int>.Success(idLastActivity);
            }
        }
    }
}
