using System;
using System.Data;
using keepr_server.Models;
using Dapper;

namespace keepr_server.Repositories
{
  public class VaultKeepRepository
  {
    private readonly IDbConnection _db;
    public VaultKeepRepository(IDbConnection db)
    {
      _db = db;
    }
    internal VaultKeep Create(VaultKeep newVaultKeep)
    {
      string sql = @"
      INSERT INTO vaultkeeps 
      (vaultId, keepId, creatorId) 
      VALUES 
      (@VaultId, @KeepId, @CreatorId);
      SELECT LAST_INSERT_ID();";
      int id = _db.ExecuteScalar<int>(sql, newVaultKeep);
      newVaultKeep.Id = id;
      return newVaultKeep;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM vaultkeeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });

    }
    internal VaultKeep GetById(int id){
      string sql = @"
      SELECT * FROM vaultkeeps WHERE id = @id;";
      return _db.QueryFirstOrDefault<VaultKeep>(sql, new { id });
    }
  }
}