﻿using Bogus;
using TeacherReviews.Domain.Entities;

namespace TeacherReviews.API.Tests;

public static class Fakers
{
    public static Faker Faker => new();

    public static Faker<City> CityFaker { get; } = new Faker<City>()
                                                   .RuleFor(x => x.Id, _ => Guid.NewGuid().ToString())
                                                   .RuleFor(x => x.Name, faker => faker.Address.City());

    public static Faker<University> UniversityFaker { get; } = new Faker<University>()
                                                               .RuleFor(u => u.Id, _ => Guid.NewGuid().ToString())
                                                               .RuleFor(u => u.Name, faker => faker.Address.City() + "University")
                                                               .RuleFor(u => u.Abbreviation, faker => faker.Random.String2(3, 4))
                                                               .RuleFor(u => u.City, _ => CityFaker.Generate())
                                                               .RuleFor(u => u.CityId, (_, u) => u.City.Id);

    public static Faker<Teacher> TeacherFaker { get; } = new Faker<Teacher>()
                                                         .RuleFor(t => t.Id, _ => Guid.NewGuid().ToString())
                                                         .RuleFor(t => t.Name, faker => faker.Name.FirstName())
                                                         .RuleFor(t => t.Surname, faker => faker.Name.LastName())
                                                         .RuleFor(t => t.Patronymic, faker => faker.Name.Random.String2(4, 8))
                                                         .RuleFor(t => t.University, _ => UniversityFaker.Generate())
                                                         .RuleFor(t => t.UniversityId, (_, t) => t.University.Id);
}