FROM microsoft/windowsservercore
RUN mkdir "C:\Program Files (x86)\Warewolf\Server"
ADD [".", "Program Files (x86)/Warewolf/Server"]
EXPOSE 3142
EXPOSE 3143
RUN NET user WarewolfUser Dev2@dmin123 /ADD
RUN NET localgroup "Administrators" WarewolfUser /ADD
RUN NET localgroup "Warewolf Administrators" /ADD
RUN NET localgroup "Warewolf Administrators" WarewolfUser /ADD
RUN sc create "Warewolf Server" binPath= "C:\Program Files (x86)\Warewolf\Server\Warewolf Server.exe" start= auto