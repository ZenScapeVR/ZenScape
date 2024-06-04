# ZenScape VR: Task Management Training

## Goal
**“Train users to improve task management skills in environments filled with sensory distractions."**

## Demo and Trailer
- **[Demo](https://youtu.be/z1XCMAiaMtU)** - View Gameplay Here!
- **[Trailer](https://youtu.be/U8CeUmOvadM)** - Just For Fun!
  
## MVP
ZenScape is an immersive VR experience designed to help users train task management in a VR office space. It provides cognitive tasks amidst various sensory distractions. The difficulty of these distractions is controlled by the user's heart rate, monitored by a pulse sensor, as heart rate is a good indicator of cognitive load which directly affects task management abilities.

## Implementation Vehicle
ZenScape is implemented using Virtual Reality (VR) and Physical Computing. We utilized Unity as our game engine to create an immersive and realistic VR experience. By simulating fine motor skills and providing realistic auditory and visual feedback, users are engaged in a highly interactive environment.

## VR Experience
ZenScape’s user experience is paramount. The VR controllers mirror the user's hand movements and gestures, allowing users to pinch or grip objects. Navigation within the environment is facilitated by a character controller and locomotion system. ZenScape features six scenes:

- **Login:** Users enter their ID or select "New User" to receive an ID.
- **Tutorial Video:** Familiarizes users with the VR environment and provides essential instructions.
- **Lobby:** Users can review metrics, watch the tutorial video, or proceed to different levels.
- **Day 1:** An introductory level with simplified tasks and distractions.
- **Day 2 and Day 3:** Increase in complexity and difficulty of tasks and distractions.

## Tasks
ZenScape includes three cognitive tasks that require focus and accuracy:

1. **Cognitive Sorting Task:** Inspired by the Stroop test, users sort binders into colored bins based on the color label, ignoring the binder's color.
2. **Coffee Mug Timing Task:** Users must fill a coffee mug to the right temperature, balancing time management skills.
3. **Phone Call Handling Task:** Users distinguish between spam and real calls based on tone and word choice.

## Distractions
Distractions are categorized into three difficulty tiers, influenced by the user’s heart rate:

- **Easy Distractions:** Fan Buzzing, Quiet Conversation, Lights Flickering
- **Medium Distractions:** Alarm Clock, Monitor/TV Static, Record Player
- **Hard Distractions:** Passing Ambulance/Sirens, Heart Beat, Construction Noise

## Physical Computing
Heart rate data is collected using an ear clip attached to the user's earlobe. The data is processed using an Arduino, which communicates with a Python script to update a Firebase Realtime Database with the user's pulse history, average pulse, and live pulse data.

## Art & Design
Assets were meticulously chosen and modeled from real and virtual office environments, including filing cabinets, desks, monitors, and more. The environment’s layout includes twists and turns, with PBR textures for a realistic feel. Concept sketches and storyboards were used to design tasks and environments such as the file sorting, coffee brewing, and phone answering tasks.

## Reflections on Goal
ZenScape addresses the challenge of maintaining focus and managing tasks amidst distractions. It helps users train their task management abilities and monitor their performance over time, equipping them with the tools needed to excel in distracting environments.

## Technologies Used
- **Unity:** Game engine used for VR development
- **Firebase:** Realtime Database for storing pulse data
- **Arduino:** Used for collecting heart rate data
- **Oculus:** VR hardware used for the user experience
- **C#:** Programming language used within Unity
- **Python:** Used for scripting and data processing
- **JIRA:** Project management tool used for tracking tasks and progress

## Team Members and Roles
- **Sadie Forbes:** Game Integration Developer
- **Harrison Juneau:** Physical Computing Specialist
- **Zavien Kellum:** Art and Game Design
- **Bryce Olivier:** Game Design and Development
- **Preston Schnell:** Lead Art and Asset Designer
- **Madelyn Zambiasi:** Storyboard and Asset Designer

---

This project was developed as part of a Digital Media Capstone Project for the LSU College of Engineering and the LSU College of Art & Design.
