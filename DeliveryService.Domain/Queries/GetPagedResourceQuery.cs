﻿namespace DeliveryService.Domain.Queries
{
    public class GetPagedResourceQuery
    {
        private const int MaxPageSize = 100;

        private int pageSize = 10;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public int Page { get; set; } = 1;

        public string Search { get; set; } = "";
    }
}
