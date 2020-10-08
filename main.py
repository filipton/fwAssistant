#!/usr/bin/env python

from __future__ import print_function

import argparse
import os.path
import json
import os
import requests
import logging

import google.oauth2.credentials
import RPi.GPIO as GPIO
from google.assistant.library import Assistant
from google.assistant.library.event import EventType
from google.assistant.library.file_helpers import existing_file
from http.server import BaseHTTPRequestHandler, HTTPServer
from threading import Thread

GPIO.setmode(GPIO.BCM)
GPIO.setup(4, GPIO.IN)

parser = argparse.ArgumentParser(
    formatter_class=argparse.RawTextHelpFormatter)
parser.add_argument('--credentials', type=existing_file,
                    metavar='OAUTH2_CREDENTIALS_FILE',
                    default=os.path.join(
                        os.path.expanduser('/root/.config'),
                        'google-oauthlib-tool',
                        'credentials.json'
                    ),
                    help='Path to store and read OAuth2 credentials')
args = parser.parse_args()
with open(args.credentials, 'r') as f:
    credentials = google.oauth2.credentials.Credentials(token=None,
                                                        **json.load(f))

assistant = Assistant(credentials, 'symbolic-heaven-291215-myhomerpi-ky4zj8')

def process_event(event):
    """Pretty prints events.
    Prints all events that occur with two spaces between each new
    conversation and a single space between turns of a conversation.
    Args:
        event(event.Event): The current event to process.
    """
    if event.type == EventType.ON_CONVERSATION_TURN_STARTED:
        req = requests.get("http://localhost:21377/stoptts")
        # if "SPEAKING" in req.content.decode("utf-8"):
        #    assistant.stop_conversation()

        print()
        GPIO.setup(4, GPIO.OUT)
        file = "/root/beep.mp3"
        os.system("mpg123 -q -f -13107 " + file)

    if event.type == EventType.ON_RECOGNIZING_SPEECH_FINISHED:
        try:
            request = requests.get("http://localhost:21377/stt/"+event.args['text'].replace("%", "&PERCENT&"))
            response = request.content.decode("utf-8")
            if 'NO RESPONSE' in response:
                assistant.stop_conversation()
        except:
            try:
                requests.get("http://localhost:21377/stt/powiedz Wystąpił niespodziewany błąd!")
                assistant.stop_conversation()
            except:
                print("An exception occurred")
                assistant.stop_conversation()
    elif event.type == EventType.ON_CONVERSATION_TURN_TIMEOUT:
        GPIO.setup(4, GPIO.IN)


    print(event)

    if (event.type == EventType.ON_CONVERSATION_TURN_FINISHED and
            event.args and not event.args['with_follow_on_turn']):
        print()
        GPIO.setup(4, GPIO.IN)


class S(BaseHTTPRequestHandler):
    def _set_response(self):
        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()

    def do_GET(self):
        path = str(self.path)
        if path == "/start":
            print("START");
            assistant.start_conversation()
        elif path == "/test":
            print("TEST")
        print(str(self.path))
        # logging.info("GET request,\nPath: %s\nHeaders:\n%s\n", str(self.path), str(self.headers))
        self._set_response()
        self.wfile.write("X".encode('utf-8'))

    def do_POST(self):
        content_length = int(self.headers['Content-Length']) # <--- Gets the size of data
        post_data = self.rfile.read(content_length) # <--- Gets the data itself
        logging.info("POST request,\nPath: %s\nHeaders:\n%s\n\nBody:\n%s\n",
                str(self.path), str(self.headers), post_data.decode('utf-8'))

        self._set_response()
        self.wfile.write("POST request for {}".format(self.path).encode('utf-8'))

def run(server_class=HTTPServer, handler_class=S, port=21378):
    logging.basicConfig(level=logging.INFO)
    server_address = ('', port)
    httpd = server_class(server_address, handler_class)
    logging.info('Starting httpd...\n')
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        pass
    httpd.server_close()
    logging.info('Stopping httpd...\n')


def main():
    thread = Thread(target = run)
    thread.start()

    for event in assistant.start():
        process_event(event)


if __name__ == '__main__':
    main()
