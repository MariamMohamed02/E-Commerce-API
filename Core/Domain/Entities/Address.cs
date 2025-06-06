﻿namespace Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        // Relationship
        public User User { get; set; }
        public string UserId { get; set; }
    }
}