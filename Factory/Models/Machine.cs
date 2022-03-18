using System.Collections.Generic;

namespace Factory.Models
{
  public class Machine
  {
    public Machine()
    {
      this.JoinEntities = new HashMap<EngineerMachine>();
    }

    public int MachineId { get; set; }
    public string Type { get; set; }
    public int YearsInService { get; set; }

    public virtual ICollection<EngineerMachine> JoinEntities { get; set; }
  }
}