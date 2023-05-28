# MAUINoiseAlertApp
The Noise Alert App is designed to enhance your work environment by providing a gentle reminder to prioritize your safety and concentration. This app reminds you to put on your noise-blocking equipment, such as ear muffs, whenever the noise levels exceed your comfort or safety threshold.


# Functionality
The App has the following functionalities:

a)Alert Frequency Slider: The slider is used by the user to set the push notification frequency while the audio monitoring is on. On dragging the Thumb across Slider, it's value is displayed below in seconds
b)Noise Threshold Slider: When the audio monitoring is active, the user can utilize the slider to adjust the noise threshold. If the ambient noise level exceeds the set threshold, it triggers an alert to notify the user. On dragging the Thumb across Slider, it's value is displayed below in db
c)Start/Stop Button: The button when pressed:
 1. First Asks user for Microphone & Notification permission, if the user has not already granted these permissions to the app
 2. Starts the audio monitoring & hence the text below the button changes to stop.
 3. When the button is pressed again, the audio monitoring stops & the text below the button changes to start.
 4. While the app monitors the audio (be it when app is opened or when app is running in background), the app checks whether the current decibel of the audio stream is above the Noise threshold & sends periodic notification as set by Alert Frequency. If the decibel is below the threshold level, the app sends a silent notification indicating current decibel level. If the decibel crosses threshold, then the app sends a vibrating notification indicating that the user should wear protective gear .Once the user clicks on stop, since the audio streaming stops , the notifications stop as well
d)Mark alerts as critical toggle: Toggle is used to mark the notifications as critical



https://github.com/makhtar18/MAUINoiseAlertApp/assets/122763616/5c9f9769-b886-4164-bb58-2dfa569b16e8

