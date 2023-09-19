using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Mission
    {
        public Planet start;
        public Planet destination;
        public int price;
        private int satisfaction = 100;
        private bool isActive = false;

        string[] greetings = { "Hello!", "Hi", "Salutations!", "Good morning!",
            "Greetings!", "Hey there!", "Howdy!", "Yo!", "What's up!", "Hiya!" };

    public bool IsActive { get => isActive; set => isActive = value; }

    public Mission(Planet start, Planet destination, int price)
        {
            this.start = start;
            this.destination = destination;
            this.price = price;

            int greetIndex = Random.Range(0, greetings.Length);
            start.SetMissionText(greetings[greetIndex] + "\n Please get me to " + destination.name + " !\n Price: " + price);
        }

        public void DecreaseSatisfaction(int number)
        {
            satisfaction -= number;
        }

        public void StartMission()
        {
            start.SetMissionStartText();
            IsActive = true;
        }

        public void EndMission()
        {
            destination.SetMissionEndText(price);
        }

}

