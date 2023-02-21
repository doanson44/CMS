using System;
using System.Threading.Tasks;
using CMS.Core.Data;
using CMS.Core.Data.Entities.Todo;
using CMS.Core.Data.Extensions;
using CMS.Core.Data.Repositories;
using CMS.Core.Domains;
using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using CMS.Core.Exceptions;
using CMS.Core.Services.Interfaces;

namespace CMS.Core.Services.Implementations;

public class TodoService : ITodoService
{
    private readonly IRepository<TodoItem> _todoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _todoRepository = unitOfWork.Get<TodoItem>();
    }

    public async Task<PagedList<TodoDto>> GetAllAsync(TodoQueryParam query)
    {
        var (page, take, search, sort, asc) = query.Params;
        var condition = PredicateBuilder.True<TodoItem>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            condition = condition.And(x => x.Description.Contains(search) || x.Item.Contains(search));
        }

        var result = await _todoRepository.GetPagedListAsync(
            condition,
            x => new TodoDto(x),
            o => o.Sort(x => x.Item, true),
            query.Page,
            query.Take);

        return result;
    }

    public async Task CreateAsync(TodoRequest request)
    {
        var todo = new TodoItem(request);
        // Check valid duplicate
        var isExist = await _todoRepository.IsLiveAsync(x => x.Item == todo.Item);
        if (isExist)
        {
            throw new BusinessException(ErrorCodes.TodoExist);
        }
        await _todoRepository.AddAsync(todo);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);

        if (todo == null)
        {
            throw new BusinessException(ErrorCodes.TodoNotFound);
        }

        _todoRepository.Remove(todo);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TodoDto> GetByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new BusinessException(ErrorCodes.TodoNotFound);
        }

        var TodoDto = new TodoDto(todo);
        return TodoDto;
    }

    public async Task UpdateAsync(int id, TodoRequest request)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new BusinessException(ErrorCodes.TodoNotFound);
        }

        todo.Item = request.Item;
        todo.Description = request.Description;
        todo.Minutes = request.Minutes;

        _todoRepository.Update(todo);
        await _unitOfWork.SaveChangesAsync();
    }
}
