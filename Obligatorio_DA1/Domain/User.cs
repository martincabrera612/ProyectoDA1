﻿namespace Domain;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
    
}