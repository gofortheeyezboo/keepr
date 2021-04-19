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
      //TODO if they are creating a vaultkeep, make sure they are the creator of the list
      return _repo.Create(newVaultKeep);

    }

    internal void Delete(int id)
    {
      //NOTE getbyid to validate its valid and you are the creator
      _repo.Delete(id);
    }
  }
}