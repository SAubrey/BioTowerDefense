using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __app : MonoBehaviour {



        // Dictionaries are arranged in order of effectiveness up to carb (1-5)
    // Linezolid is its own category, rifampicin and isoniazid are their own category.
    private static IDictionary<string, float> amox = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> meth = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> vanc = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> carb = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", 1f}, // best. If resistant, 1, 2, 3 useless.
                                        {"TB", 0f} };
    private static IDictionary<string, float> line = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .9f}, // second best
                                        {"TB", 0f} };
    private static IDictionary<string, float> rifa = new Dictionary<string, float>() {
                                        {"staph", .1f},
                                        {"strep", .1f},
                                        {"pneu", .1f},
                                        {"TB", .5f} };
    private static IDictionary<string, float> ison = new Dictionary<string, float>() {
                                        {"staph", .1f},
                                        {"strep", .1f},
                                        {"pneu", .1f},
                                        {"TB", .5f} };                                                                                
   
    public IDictionary<string, IDictionary<string, float>> antibiotics = new Dictionary<string, IDictionary<string, float>>() {
                                        {"amox", amox},
                                        {"meth", meth},
                                        {"vanc", vanc},
                                        {"carb", carb},
                                        {"line", line},
                                        {"rifa", rifa},
                                        {"ison", ison} };

	public float baseMutationChance = 0f;
    public float mutationIncrement = 0.05f;

    public static IDictionary<string, float> staphChances = new Dictionary<string, float>() {
                                    {"amox", 0f},
                                    {"meth", 0f},
                                    {"vanc", 0f},
                                    {"carb", 0f},
                                    {"line", 0f},
                                    {"rifa", 0f},
                                    {"ison", 0f} };
    public static IDictionary<string, float> strepChances = new Dictionary<string, float>() {
                                    {"amox", 0f},
                                    {"meth", 0f},
                                    {"vanc", 0f},
                                    {"carb", 0f},
                                    {"line", 0f},
                                    {"rifa", 0f},
                                    {"ison", 0f} };

    public static IDictionary<string, float> pneuChances = new Dictionary<string, float>() {
                                    {"amox", 0f},
                                    {"meth", 0f},
                                    {"vanc", 0f},
                                    {"carb", 0f},
                                    {"line", 0f},
                                    {"rifa", 0f},
                                    {"ison", 0f} };

    public static IDictionary<string, float> TBChances = new Dictionary<string, float>() {
                                    {"amox", 0f},
                                    {"meth", 0f},
                                    {"vanc", 0f},
                                    {"carb", 0f},
                                    {"line", 0f},
                                    {"rifa", 0f},
                                    {"ison", 0f} };

     public IDictionary<string, IDictionary<string, float>> mutationChances = 
                                    new Dictionary<string, IDictionary<string, float>>() {
                                        {"staph", staphChances},
                                        {"strep", strepChances},
                                        {"pneu", pneuChances},
                                        {"TB", TBChances} };
            


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void increaseMutationChance(string species, string antibioticType) {
        // Called by Enemy on death. Increase mutation chance by mutationIncrement relative to the bacteria species/antibioticType
        mutationChances[species][antibioticType] += mutationIncrement;
	}
}
