using System;
using System.Collections.Generic;
using aKnightsTale.Models;
using aKnightsTale.Repositories;

namespace aKnightsTale.Services
{
  public class KnightsService
  {
    private readonly KnightsRepository _repo;

    public KnightsService(KnightsRepository repo)
    {
      _repo = repo;
    }

    internal List<Knight> Get()
    {
      return _repo.Get();
    }

    internal Knight Get(int id)
    {
      Knight found = _repo.Get(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal Knight Create(Knight knightData)
    {
      return _repo.Create(knightData);
    }

    internal Knight Edit(Knight knightData)
    {
      Knight original = Get(knightData.Id);
      original.Name = knightData.Name ?? original.Name;

      _repo.Edit(original);

      return original;
    }

    internal Knight Delete(int id, string userId)
    {
      Knight original = Get(id);
      _repo.Delete(id);

      return original;
    }
  }
}