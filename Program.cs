﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

class Program
{
  static void Main()
  {
    int port = 5000;

    var server = new Server(port);

    Console.WriteLine("The server is running");
    Console.WriteLine($"Main Page: http://localhost:{port}/website/pages/index.html");

    var database = new Database();

    if (true)
    {
      database.Catagories.Add(new Catagory("clothing"));

      database.Catagories.Add(new Catagory("fruits"));
      database.Products.Add(new Product("Apple", 2));
      database.Products.Add(new Product("Banana", 2));

      database.Catagories.Add(new Catagory("Meats"));
      database.Products.Add(new Product("Steak", 3));
      database.Products.Add(new Product("Beef", 3));
      database.SaveChanges();
      database.Catagories.Remove(database.Catagories.Find(1)!);
      database.SaveChanges();
    }


    while (true)
    {
      (var request, var response) = server.WaitForRequest();

      Console.WriteLine($"Recieved a request with the path: {request.Path}");

      if (File.Exists(request.Path))
      {
        var file = new File(request.Path);
        response.Send(file);
      }
      else if (request.ExpectsHtml())
      {
        var file = new File("website/pages/404.html");
        response.SetStatusCode(404);
        response.Send(file);
      }
      else
      {
        try
        {
          /*──────────────────────────────────╮
          │ Handle your custome requests here │
          ╰──────────────────────────────────*/
          if (request.Path == "getCatagories")
          {
            string[] catagoriesTitles =
              database.Catagories.Select(catagory => catagory.Title).ToArray();

            int[] catagoriesIds =
              database.Catagories.Select(catagory => catagory.Id).ToArray();


            response.Send((catagoriesIds, catagoriesTitles));
          }
          if (request.Path == "getCatagoryTitle")
          {
            int catagoryId = request.GetBody<int>();

            Catagory catagory = database.Catagories.Find(catagoryId)!;

            response.Send(catagory.Title);
          }
          else
          {
            response.SetStatusCode(405);
          }

          database.SaveChanges();
        }
        catch (Exception exception)
        {
          Log.WriteException(exception);
        }
      }

      response.Close();
    }
  }
}


class Database() : DbBase("database")
{
  /*──────────────────────────────╮
  │ Add your database tables here │
  ╰──────────────────────────────*/
  public DbSet<User> Users { get; set; } = default!;
  public DbSet<Catagory> Catagories { get; set; } = default!;
  public DbSet<Product> Products { get; set; } = default!;
}

class User(string username, string password)
{
  [Key] public int Id { get; set; } = default!;
  public string Username { get; set; } = username;
  public string Password { get; set; } = password;
}

class Catagory(string title)
{
  [Key] public int Id { get; set; } = default!;
  public string Title { get; set; } = title;
}

class Product(string name, int catagoryId)
{
  [Key] public int Id { get; set; } = default!;
  public string Name { get; set; } = name;

  public int CatagoryId { get; set; } = catagoryId;
  [ForeignKey("CatagoryId")] public Catagory Catagory { get; set; } = default!;
}