using System;
using System.Collections.Generic;
using System.Linq;
using keepr_server.Models;
using keepr_server.Repositories;
using System.Web;

namespace keepr_server.Services
{
  public class VaultService
  {
    private readonly VaultRepository _repo;

    public VaultService(VaultRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Vault> Get()
    {
      return _repo.GetAll();
    }

    internal Vault GetById(int id, string userId)
    {
      Vault data = _repo.GetById(id);
      if (data == null)
      {
        throw new Exception("Invalid id");
      } else if(data.CreatorId != userId && data.IsPrivate == true){
        throw new Exception("You can only view your own private vaults");
      } else {
        return data;
      }
    }

    internal Vault Create(Vault newVault)
    {
      return _repo.Create(newVault);
    }
    internal Vault Delete(int id, string userId)
    {
      Vault original = GetById(id, userId);
      if (userId != original.CreatorId)
      {
        throw new Exception("You can only delete your own data");
      }
      _repo.Delete(id);
      return original;
    }
    internal Vault Edit(Vault updated)
    {
      Vault original = _repo.GetById(updated.Id);
      if (updated.CreatorId != original.CreatorId)
      {
        throw new Exception("You can only edit your own data");
      }
      updated.Name = updated.Name != null ? updated.Name : original.Name;
      updated.Description = updated.Description != null ? updated.Description : original.Description;

      return _repo.Edit(updated);
    }
    internal IEnumerable<VaultKeepViewModel> GetVaultsByProfileId(string id){
    IEnumerable<VaultKeepViewModel> vaults = _repo.GetVaultsByProfileId(id);
    return vaults.ToList().FindAll(v => v.IsPrivate == false);
    }
  }
}