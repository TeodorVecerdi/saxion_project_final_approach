- When the Game project builds it copies Game/data and Game/lib to the output folder overriding existing files.
- GXPEngine is used as a library and is kept as a separate project (it's built and copied to the output folder as well when the Game project builds).
- To switch the networking from localhost to our hosted server edit data/hosted.txt and change 0 (local) to 1 (hosted).

While testing with my team members the game kept crashing because of weird reasons.
Some of the reasons were: 
- one of the people had Bandicam running and recording the screen and after stopping bandicam completely it worked. (I'm still not sure if this was the problem)
- another person while running in Release mode got the error messages: "Unable to create OpenGL context" and "Failed to create GLFW window", then in Debug mode it started but after getting to the loading screen it crashed. I'm not sure what was causing this issue but a poor internet connection might be an issue. The only thing I know is that the socket.io connection was closed because of 'ping timeout'.
- another reason to crash might be because the project is targeted at the v4.8 .NET Framework if you don't have it installed

It worked perfectly on two of my machines ( :) ) and on a friend's laptop but this might be because we have everything installed to the latest version, including the .net framework, and we're both running windows.

Also, the project is so big (~150MB) because of the songs we have. They're around 100MB.