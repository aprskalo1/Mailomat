using Mailomat.Integrations.SudReg.Models;

namespace Mailomat.Integrations.SudReg.Interfaces;

public interface ISubjektiService
{
    Task<IEnumerable<SubjektResponse>> GetSubjektiAsync(int snapshotId, int offset, int limit, bool onlyActive = true);
}