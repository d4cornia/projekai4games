using Bolt;
using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UnitTitle("Set Objective")]
[UnitCategory("Objective")]
public class setObjective : Unit
{
    public GameObject objText;

    // inisiasi input
    [DoNotSerialize]
    public ControlInput input { get; private set; }

    // inisiasi output
    [DoNotSerialize]
    public ControlOutput output { get; private set; }

    // inisiasi variable
    [DoNotSerialize]
    public ValueInput objIn { get; private set; }

    protected override void Definition()
    {
        // assign input dengan label in dan menjalankan function Enter
        input = ControlInput("In", Enter);
        // assign output dengan label out
        output = ControlOutput("Out");

        objIn = ValueInput<List<string>>("Objective");

        // jika memakai block ini wajib mengisi objIn dan input
        Requirement(objIn, input);
    }

    public ControlOutput Enter(Flow flow)
    {
        List<string> obj = flow.GetValue<List<string>>(objIn);
        string hasil = "";
        foreach (var item in obj)
        {
            hasil += item + "\n";
        }
        if(GameObject.Find("obj text") != null)
        {
            GameObject.Find("obj text").GetComponent<Text>().text = hasil;
        }

        return output;
    }

}
