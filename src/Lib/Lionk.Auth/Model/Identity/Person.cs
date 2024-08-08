// Copyright © 2024 Lionk Project

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a person.
/// </summary>
public class Person
{
    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the birth date of the person.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class.
    /// </summary>
    /// <param name="firstName"> The first name of the person.</param>
    /// <param name="lastName"> The last name of the person.</param>
    /// <param name="birthDate"> The birth date of the person.</param>
    public Person(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    /// <summary>
    /// Gets the full name of the person.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
