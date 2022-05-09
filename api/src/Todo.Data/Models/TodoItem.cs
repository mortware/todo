namespace Todo.Api.Requests;

using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    [Required]
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }
    
    public string Text { get; set; }
    
    public DateTime? Completed { get; set; }
}