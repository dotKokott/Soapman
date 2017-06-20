#pragma once

#include "ofMain.h"
#include "ofxCv.h"
#include "ofxGui.h"
#include "ofxOsc.h"

#define OSC_HOST	"localhost"
#define OSC_PORT	12345

#define CAM_WIDTH	640
#define CAM_HEIGHT	480

class ofApp : public ofBaseApp {
public:
    void setup();
    void update();
    void draw();
	void mouseMoved(int x, int y);
	void keyPressed(int key);

	int mX;
	int mY;
    
    ofVideoGrabber cam;
    ofxCv::ContourFinder gloveFinder;
    ofColor gloveColor;

	ofxCv::ContourFinder object1Finder;
	ofColor object1Color;

	ofxCv::ContourFinder object2Finder;
	ofColor object2Color;

	ofxCv::ContourFinder object3Finder;
	ofColor object3Color;

    ofxPanel gui;
    ofParameter<float> gloveTreshold;
	ofParameter<float> object1Treshold;
	ofParameter<float> object2Treshold;
	ofParameter<float> object3Treshold;

    ofParameter<bool> trackHs;

	ofxOscSender sender;
	vector<string> arguments;
};
