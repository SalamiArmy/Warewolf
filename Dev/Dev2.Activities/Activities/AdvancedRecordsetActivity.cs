using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev2.Interfaces;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Activities
{
	public class AdvancedRecordsetActivity : DsfActivityAbstract<string>, IEquatable<AdvancedRecordsetActivity>
	{
		public bool Equals(AdvancedRecordsetActivity other)
		{
			throw new NotImplementedException();
		}

		public override IList<DsfForEachItem> GetForEachInputs()
		{
			throw new NotImplementedException();
		}

		public override IList<DsfForEachItem> GetForEachOutputs()
		{
			throw new NotImplementedException();
		}

		public override List<string> GetOutputs()
		{
			throw new NotImplementedException();
		}

		public override void UpdateForEachInputs(IList<Tuple<string, string>> updates)
		{
			throw new NotImplementedException();
		}

		public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates)
		{
			throw new NotImplementedException();
		}

		protected override void ExecuteTool(IDSFDataObject dataObject, int update)
		{
			throw new NotImplementedException();
		}

		protected override void OnExecute(NativeActivityContext context)
		{
			throw new NotImplementedException();
		}
	}
}
