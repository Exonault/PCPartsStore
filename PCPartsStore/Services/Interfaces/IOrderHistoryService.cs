﻿using AspStore.ViewModels;
using PCPartsStore.Paging;

namespace PCPartsStore.Services.Interfaces;

public interface IOrderHistoryService
{
    public PaginatedList<OrderHistoryViewModel> OrderPage(int? page);
    public OrderHistoryDetailsViewModel OrderDetailsPage(int id);
}