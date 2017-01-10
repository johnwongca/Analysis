using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CommonCLR
{
	enum SequenceType { String, Integer, Binary }
	internal class SequenceItem
	{
		public SequenceItem(string name, SequenceType type, bool recycle, string startWith, long startValue, long endValue, long currentValue, string characters, bool padLeft,	bool active)		
		{
			this.name = name; this.type = type; this.recycle = recycle;
			this.startWith = startWith; this.startValue = startValue; this.endValue = endValue;
			this.currentValue = currentValue; this.characters = characters; this.padLeft = padLeft;
			this.active = active;
			Validate();
		}
		public SequenceItem(string name, string type, bool recycle, string startWith, long startValue, long endValue, long currentValue, string characters, bool padLeft, bool active)
		{
			this.name = name; this.type = (SequenceType)Enum.Parse(typeof(SequenceType), type, true); this.recycle = recycle;
			this.startWith = startWith; this.startValue = startValue; this.endValue = endValue;
			this.currentValue = currentValue; this.characters = characters; this.padLeft = padLeft;
			this.active = active;
			Validate();
		}
		~SequenceItem()
		{
			throw new System.NotImplementedException();
		}

		private string name;
		private SequenceType type;
		private bool recycle;
		private string startWith;
		private long startValue;
		private long endValue;
		private long currentValue;
		private string characters;
		private bool padLeft;
		private bool active;
		private SqlConnection connection = null;

		public string Name { get { return name; } set { name = value; Validate(); } }
		public SequenceType Type { get { return type; } set { type = value; Validate(); } }
		public bool Recycle { get { return recycle; } set { recycle = value; Validate(); } }
		public string StartWith { get { return startWith; } set { startWith = value; Validate(); } }
		public long StartValue { get { return startValue; } set { startValue = value; Validate(); } }
		public long EndValue { get { return endValue; } set { endValue = value; Validate(); } }
		public long CurrentValue { get { return currentValue; } set { currentValue = value; Validate(); } }
		public string Characters { get { return characters; } set { characters = value; Validate(); } }
		public bool PadLeft { get { return padLeft; } set { padLeft = value; Validate(); } }
		public bool Active { get { return active; } set { active = value; Validate(); } }
		public SqlConnection Connection { get { return connection; } set { connection = value; } }

		private void Validate()
		{
			throw new System.NotImplementedException();
		}

	}
}
