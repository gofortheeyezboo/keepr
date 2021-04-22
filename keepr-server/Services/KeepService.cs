using System;
using System.Collections.Generic;
using System.Linq;
using keepr_server.Models;
using keepr_server.Repositories;

namespace keepr_server.Services
{
  public class KeepService
  {
    private readonly KeepRepository _repo;

    public KeepService(KeepRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Keep> Get()
    {
      return _repo.GetAll();
    }

    internal Keep GetById(int id)
    {
      Keep data = _repo.GetById(id);
      if (data == null)
      {
        throw new Exception("Invalid id");
      }
      return data;
    }

    internal Keep Create(Keep newKeep)
    {
      return _repo.Create(newKeep);
    }
    internal Keep Delete(int id, string userId)
    {
      Keep original = GetById(id);
      if (userId != original.CreatorId)
      {
        throw new Exception("You can only delete your own data");
      }
      _repo.Delete(id);
      return original;
    }
    internal Keep Edit(Keep updated)
    {
      Keep original = GetById(updated.Id);
      if (updated.CreatorId != original.CreatorId)
      {
      updated.Views = updated.Views != null ? updated.Views : original.Views;
      updated.Shares = updated.Shares != null ? updated.Shares : original.Shares;
        throw new Exception("You can only edit your own data");
      }
      updated.Name = updated.Name != null ? updated.Name : original.Name;
      updated.Description = updated.Description != null ? updated.Description : original.Description;
      updated.Keeps = updated.Keeps != null ? updated.Keeps : original.Keeps;
      updated.Shares = updated.Shares != null ? updated.Shares : original.Shares;
      updated.Views = updated.Views != null ? updated.Views : original.Views;

      return _repo.Edit(updated);
    }

    internal IEnumerable<Keep> GetKeepsByAccountId(string id) {
      return _repo.GetKeepsByAccountId(id);
    }

    internal IEnumerable<KeepVaultViewModel> GetKeepsByVaultId(int id){
      return _repo.GetKeepsByVaultId(id);
    }
    internal IEnumerable<KeepVaultViewModel> GetKeepsByProfileId(string id){
        IEnumerable<KeepVaultViewModel> keeps = _repo.GetKeepsByProfileId(id);
        return keeps;
    }
  }
}