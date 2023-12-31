﻿using System.Linq.Expressions;
using TeacherReviews.API.Contracts.Repositories;
using TeacherReviews.API.Repositories;
using TeacherReviews.Domain;
using TeacherReviews.Domain.Entities;
using TeacherReviews.Domain.Exceptions;

namespace TeacherReviews.API.Services;

public class CityService
{
    private readonly ICityRepository _cityRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly IUniversityRepository _universityRepository;

    public CityService(ApplicationDbContext context, UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _cityRepository = new CityRepository(context);
        _universityRepository = new UniversityRepository(context);
    }

    public async Task<City> GetByIdAsync(string id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city is null)
        {
            throw new EntityNotFoundException(typeof(City), id);
        }

        return city;
    }

    public Task<IEnumerable<City>> GetAllAsync(Expression<Func<City, bool>>? filter = null)
    {
        return _cityRepository.GetAllAsync(filter);
    }

    public async Task<City> CreateAsync(City item)
    {
        if ((await _cityRepository.GetAllAsync(c => c.Name == item.Name)).Any())
        {
            throw new EntityExistsException(typeof(City), nameof(City.Name), item.Name);
        }

        var city = await _cityRepository.AddAsync(item);

        await _unitOfWork.SaveChangesAsync();

        return city;
    }

    public async Task<City> DeleteAsync(string id)
    {
        if (await _cityRepository.GetByIdAsync(id) is null)
        {
            throw new EntityNotFoundException(typeof(City), id);
        }

        var city = await _cityRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();

        return city;
    }

    public async Task<City> UpdateAsync(City item)
    {
        if (await _cityRepository.GetByIdAsync(item.Id) is null)
        {
            throw new EntityNotFoundException(typeof(City), item.Id);
        }

        if ((await _cityRepository.GetAllAsync(c => c.Name == item.Name)).Any())
        {
            throw new EntityExistsException(typeof(City), nameof(City.Name), item.Name);
        }

        var city = await _cityRepository.UpdateAsync(item);

        await _unitOfWork.SaveChangesAsync();

        return city;
    }

    public async Task<IEnumerable<University>> GetCitysUniversitiesAsync(string id)
    {
        if (await _cityRepository.GetByIdAsync(id) is null)
        {
            throw new EntityNotFoundException(typeof(City), id);
        }

        return await _universityRepository.GetAllAsync(u => u.CityId == id);
    }
}