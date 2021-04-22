using System;
using keepr_server.Models;
using keepr_server.Repositories;

namespace keepr_server.Services
{
  public class VaultKeepService
  {
    private readonly VaultKeepRepository _repo;

    public VaultKeepService(VaultKeepRepository repo)
    {
      _repo = repo;
    }

    internal VaultKeep Create(VaultKeep newVaultKeep)
    {
      
      return _repo.Create(newVaultKeep);

    }

    internal void Delete(int id, string userId)
    {
      VaultKeep vaultkeep = _repo.GetById(id);
      if(vaultkeep == null){
        throw new Exception("Invalid ID");
      }
      if(vaultkeep.CreatorId != userId){
        throw new Exception("You can only interact with your own private data");
      }
      _repo.Delete(id);
    }
  }
}