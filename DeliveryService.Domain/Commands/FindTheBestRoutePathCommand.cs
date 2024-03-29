﻿using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;

namespace DeliveryService.Domain.Commands
{
    public class FindTheBestRoutePathCommand : BaseCommand<DomainResult<BestRoutePath>>
    {
        public string RouteId { get; set; }

        public FindTheBestRoutePathCommand(string id)
        {
            RouteId = id;
        }
    }
}
