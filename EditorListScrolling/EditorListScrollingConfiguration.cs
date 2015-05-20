using System.Xml.Serialization;

namespace EditorListScrolling
{
	[XmlRootAttribute("EditorListScrollingConfig", Namespace = "KSP-Forum", IsNullable = false)]
	public class EditorListScrollingConfiguration
	{

		public object clone()
		{
			return this.MemberwiseClone();
		}

		private bool _invertMouseWheel;
		private float _mouseWheelSensitivity;
		private bool _advancedDebugging;

		public bool invertMouseWheel
		{
			get { return _invertMouseWheel; }
			set { _invertMouseWheel = value; }
		}

		public float mouseWheelSensitivity
		{
			get { return _mouseWheelSensitivity; }
			set { _mouseWheelSensitivity = value; }
		}

		public bool advancedDebugging
		{
			get { return _advancedDebugging; }
			set { _advancedDebugging = value; }
		}

	}
}
