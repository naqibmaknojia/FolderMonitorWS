# FolderMonitorWS

Folder Monitor client is supposed to track changes on a specific folder. You are required to create two
windows services: 
a) First service is supposed to check the specified folder every minute for any changes. If there are
any changes, it should copy the new or updated files to another fixed location. If there are no
changes then it should increase the delay by additional 2 minutes until it reaches 1-hour delay. The
delay should not exceed 1-hour gap. 
b) Second service is supposed to send email to the local user (email address would be in the
configuration file). This service will check for changes in the specified folder every 15 minutes and
if there is any change, it will notify the user with the filename and file Size. 
