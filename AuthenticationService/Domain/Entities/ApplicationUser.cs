﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Domain.Entities;

public class ApplicationUser
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public required string Nome { get; set; }
    [Required]
    [StringLength(50)]
    public required string Email { get; set; }
    [Required]
    [StringLength(100)]
    public required string Senha { get; set; }
    [Required]
    [StringLength(16)]
    public string? Telefone { get; set; }
    [Required]
    [StringLength(50)]
    public string? Cidade { get; set; }
}
