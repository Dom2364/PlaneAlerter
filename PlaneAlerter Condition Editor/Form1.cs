using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace PlaneAlerter_Condition_Editor {
	public partial class Form1 :Form {
		public Form1() {
			InitializeComponent();

			//LEGEND
			//A = Equals/Not Equals
			//B = Higher Than + Lower Than
			//C = True/False Boolean
			//D = Starts With + Ends With
			//E = Contains

			Core.vrsPropertyData.Add(Core.vrsProperty.Id, new string[] {"Number", "A", "The unique identifier of the aircraft." });
			Core.vrsPropertyData.Add(Core.vrsProperty.TSecs, new string[] {"Number", "AB", "The number of seconds that the aircraft has been tracked for." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Rcvr, new string[] {"Number", "A", "The ID of the feed that last supplied information about the aircraft." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Icao, new string[] {"String", "ADE", "The ICAO of the aircraft." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Bad, new string[] {"Boolean", "C", "True if the ICAO is known to be invalid." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Reg, new string[] {"String", "ADE", "The registration." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Alt, new string[] {"Number", "B", "The altitude in feet at standard pressure." });
			Core.vrsPropertyData.Add(Core.vrsProperty.AltT, new string[] {"Number", "A", "0 = altitude is barometric, 1 = altitude is geometric. Default to barometric until told otherwise." });
			Core.vrsPropertyData.Add(Core.vrsProperty.TAlt, new string[] {"Number", "AB", "The target altitude, in feet, set on the autopilot / FMS etc." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Call, new string[] {"String", "ADE", "The callsign." });
			Core.vrsPropertyData.Add(Core.vrsProperty.CallSus, new string[] {"Boolean", "C", "True if the callsign may not be correct." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Spd, new string[] {"Number", "B", "The ground speed in knots." });
			Core.vrsPropertyData.Add(Core.vrsProperty.SpdTyp, new string[] {"Number", "A", "The type of speed that Spd represents. Only used with raw feeds. 0/missing = ground speed, 1 = ground speed reversing, 2 = indicated air speed, 3 = true air speed." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Vsi, new string[] {"Number", "B", "Vertical speed in feet per minute." });
			Core.vrsPropertyData.Add(Core.vrsProperty.VsiT, new string[] {"Number", "A", "0 = vertical speed is barometric, 1 = vertical speed is geometric. Default to barometric until told otherwise." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Mdl, new string[] {"String", "AE", "A description of the aircraft's model." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Type, new string[] {"String", "AE", "The aircraft model's ICAO type code." });
			Core.vrsPropertyData.Add(Core.vrsProperty.From, new string[] {"String", "AE", "The code and name of the departure airport." });
			Core.vrsPropertyData.Add(Core.vrsProperty.To, new string[] {"String", "AE", "The code and name of the arrival airport." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Op, new string[] {"String", "AE", "The name of the aircraft's operator." });
			Core.vrsPropertyData.Add(Core.vrsProperty.OpCode, new string[] {"String", "A", "The operator's ICAO code." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Sqk, new string[] {"Number", "A", "The squawk." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Help, new string[] {"Boolean", "C", "True if the aircraft is transmitting an emergency squawk." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Dst, new string[] {"Number", "B", "The distance to the aircraft in kilometres." });
			Core.vrsPropertyData.Add(Core.vrsProperty.WTC, new string[] {"Number", "A", "The wake turbulence category of the aircraft. 1 = none, 2 = light, 3 = medium, 4 = heavy" });
			Core.vrsPropertyData.Add(Core.vrsProperty.Engines, new string[] {"Number", "AB", "The number of engines the aircraft has." });
			Core.vrsPropertyData.Add(Core.vrsProperty.EngType, new string[] {"Number", "A", "The type of engine the aircraft uses. 0 = none, 1 = piston, 2 = turbo, 3 = jet, 4 = electric" });
			Core.vrsPropertyData.Add(Core.vrsProperty.Species, new string[] {"Number", "A", "The species of the aircraft (helicopter, jet etc.). 0 = none, 1 = landplane, 2 = seaplane, 3 = amphibian, 4 = helicopter, 5 = gyrocopter, 6 = tiltwing, 7 = ground vehicle, 8 = tower" });
			Core.vrsPropertyData.Add(Core.vrsProperty.Mil, new string[] {"Boolean", "C", "True if the aircraft appears to be operated by the military." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Cou, new string[] {"String", "A", "The country that the aircraft is registered to." });
			Core.vrsPropertyData.Add(Core.vrsProperty.FlightsCount, new string[] {"Number", "AB", "The number of Flights records the aircraft has in the database." });
			Core.vrsPropertyData.Add(Core.vrsProperty.CMsgs, new string[] {"Number", "AB", "The count of messages received for the aircraft." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Gnd, new string[] {"Boolean", "C", "True if the aircraft is on the ground." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Tag, new string[] {"String", "AE", "The user tag found for the aircraft in the BaseStation.sqb local database." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Interested, new string[] {"Boolean", "C", "True if the aircraft is flagged as interesting in the BaseStation.sqb local database." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Trt, new string[] {"Number", "A", "Transponder type. 0 = Unknown, 1 = Mode-S, 2 = ADS-B (unknown version), 3 = ADS-B 1, 4 = ADS-B 2." });
			Core.vrsPropertyData.Add(Core.vrsProperty.HasSig, new string[] {"Boolean", "C", "True if the aircraft has a signal level associated with it." });
			Core.vrsPropertyData.Add(Core.vrsProperty.Sig, new string[] {"Number", "B", "The signal level for the last message received from the aircraft, as reported by the receiver. Not all receivers pass signal levels. The value's units are receiver-dependent." });

			Core.comparisonTypes.Add("A", new string[] { "Equals", "Not Equals" });
			Core.comparisonTypes.Add("B", new string[] { "Higher Than", "Lower Than" });
			Core.comparisonTypes.Add("C", new string[] { "Equals", "Not Equals" });
			Core.comparisonTypes.Add("D", new string[] { "Starts With", "Ends With" });
			Core.comparisonTypes.Add("E", new string[] { "Contains" });
			
			if (File.Exists("conditions.json")) {
				string conditionsJson = File.ReadAllText("conditions.json");
				 JsonConvert.DeserializeObject(conditionsJson);
			}
		}

		public void updateConditionList() {
			conditionTreeView.Nodes.Clear();
			foreach (Condition condition in Core.conditions.Values) {
				TreeNode conditionNode = conditionTreeView.Nodes.Add("Name: " + condition.conditionName);
				conditionNode.Nodes.Add("Id: " + condition.id);
				conditionNode.Nodes.Add("Email Parameter: " + condition.emailProperty.ToString());
				TreeNode triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (object[] trigger in condition.triggers.Values) {
					triggersNode.Nodes.Add(trigger[0] + " " + trigger[1] + " " + trigger[2]);
				}
			}
		}

		private void addConditionButton_Click(object sender, EventArgs e) {
			Condition_Editor editor = new Condition_Editor();
			editor.ShowDialog();
			updateConditionList();
		}
		
		void ConditionTreeViewNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Text.Substring(0, 6) != "Name: ") {
				return;
			}
			int conditionId = Convert.ToInt32(e.Node.Nodes[0].Text.Substring(4));
			Condition conditionToEdit = Core.conditions[conditionId];
			Condition_Editor editor = new Condition_Editor(conditionToEdit);
			editor.ShowDialog();
		}
		
		void ExitButtonClick(object sender, EventArgs e)
		{
			string conditionsJson = JsonConvert.SerializeObject(Core.conditions);
			File.WriteAllText("conditions.json", conditionsJson);
			Application.Exit();
		}
	}

	public class Condition {
		public string conditionName;
		public int id;
		public Core.vrsProperty emailProperty;
		public Dictionary<int, object[]> triggers = new Dictionary<int, object[]>();

		public Condition() {
			conditionName = "New Condition";
		}
	}

	public static class Core {
		public static Dictionary<int, Condition> conditions = new Dictionary<int, Condition>();
		public static Dictionary<vrsProperty, string[]> vrsPropertyData = new Dictionary<vrsProperty, string[]>();
		public static Dictionary<string, string[]> comparisonTypes = new Dictionary<string, string[]>();

		public enum vrsProperty {
			Id,
			TSecs,
			Rcvr,
			Icao,
			Bad,
			Reg,
			Alt,
			AltT,
			TAlt,
			Call,
			CallSus,
			Spd,
			SpdTyp,
			Vsi,
			VsiT,
			Mdl,
			Type,
			From,
			To,
			Op,
			OpCode,
			Sqk,
			Help,
			Dst,
			WTC,
			Engines,
			EngType,
			Species,
			Mil,
			Cou,
			FlightsCount,
			CMsgs,
			Gnd,
			Tag,
			Interested,
			Trt,
			HasSig,
			Sig
		}
	}
}
