using System.Collections.Generic;

namespace Factory.Models
{
  public class Engineer
  {
    public Engineer()
    {
      this.JoinEntities = new HashMap<EngineerMachine>();
    }

    public int EngineerId { get; set; }
    public string Name { get; set; }
    public int YearsExperience { get; set; }

    public virtual ICollection<EngineerMachine> JoinEntities { get; set; }
  }
}