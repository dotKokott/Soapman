#include "ofApp.h"

using namespace ofxCv;
using namespace cv;
using namespace std;

void ofApp::setup() {
	if (arguments.size() > 1) {
		cam.setDeviceID(std::stoi(arguments[1]));
	}
	else {
		std::cout << "No device specified, trying first webcam" << std::endl;
		std::cout << "If you want to specify a different device call the application with a parameter for the device index" << std::endl;
		std::cout << "Like: cmd:> tracker.exe 1" << std::endl;
		cam.setDeviceID(cam.listDevices()[0].id);
	}
	
	cam.setup(CAM_WIDTH, CAM_HEIGHT);
	
    gloveFinder.setMinAreaRadius(10);
	gloveFinder.setSortBySize(true);

	object1Finder.setMinAreaRadius(10);
	object1Finder.setSortBySize(true);

	object2Finder.setMinAreaRadius(10);
	object2Finder.setSortBySize(true);

	object3Finder.setMinAreaRadius(10);
	object3Finder.setSortBySize(true);
    
    gui.setup();
    gui.add(gloveTreshold.set("Glove Threshold", 28.05f, 0, 255));
	gui.add(object1Treshold.set("Object1 Treshold", 28.05f, 0, 255));
	gui.add(object2Treshold.set("Object2 Treshold", 28.05f, 0, 255));
	gui.add(object3Treshold.set("Object3 Treshold", 28.05f, 0, 255));
    gui.add(trackHs.set("Track Hue/Saturation", false));

	sender.setup(OSC_HOST, OSC_PORT);
}

void ofApp::update() {
    cam.update();
    if(cam.isFrameNew()) {
		ofxOscMessage info_msg;
		info_msg.setAddress("/info");

		info_msg.addInt32Arg(CAM_WIDTH);
		info_msg.addInt32Arg(CAM_HEIGHT);

		sender.sendMessage(info_msg, false);

		gloveFinder.setTargetColor(gloveColor, trackHs ? TRACK_COLOR_HS : TRACK_COLOR_RGB);
		gloveFinder.setThreshold(gloveTreshold);
		gloveFinder.findContours(cam);		

		if (gloveFinder.size() > 0) {
			ofxOscMessage glove_msg;
			
			glove_msg.setAddress("/glove");
			auto center = gloveFinder.getCenter(0);
			glove_msg.addFloatArg(center.x);
			glove_msg.addFloatArg(center.y);

			sender.sendMessage(glove_msg, false);
		}	
		else {
			ofxOscMessage glove_msg;

			glove_msg.setAddress("/!glove");

			sender.sendMessage(glove_msg, false);
		}

		object1Finder.setTargetColor(object1Color, trackHs ? TRACK_COLOR_HS : TRACK_COLOR_RGB);
		object1Finder.setThreshold(object1Treshold);
		object1Finder.findContours(cam);

		if (object1Finder.size() > 0) {
			ofxOscMessage obj1_msg;

			obj1_msg.setAddress("/object1");
			auto center = object1Finder.getCenter(0);
			obj1_msg.addFloatArg(center.x);
			obj1_msg.addFloatArg(center.y);

			sender.sendMessage(obj1_msg, false);
		}
		else {
			ofxOscMessage obj1_msg;

			obj1_msg.setAddress("/!object1");

			sender.sendMessage(obj1_msg, false);
		}

		object2Finder.setTargetColor(object2Color, trackHs ? TRACK_COLOR_HS : TRACK_COLOR_RGB);
		object2Finder.setThreshold(object2Treshold);
		object2Finder.findContours(cam);

		if (object2Finder.size() > 0) {
			ofxOscMessage obj2_msg;

			obj2_msg.setAddress("/object2");
			auto center = object2Finder.getCenter(0);
			obj2_msg.addFloatArg(center.x);
			obj2_msg.addFloatArg(center.y);

			sender.sendMessage(obj2_msg, false);
		}	
		else {
			ofxOscMessage obj2_msg;

			obj2_msg.setAddress("/!object2");

			sender.sendMessage(obj2_msg, false);
		}

		object3Finder.setTargetColor(object3Color, trackHs ? TRACK_COLOR_HS : TRACK_COLOR_RGB);
		object3Finder.setThreshold(object3Treshold);
		object3Finder.findContours(cam);

		if (object3Finder.size() > 0) {
			ofxOscMessage obj3_msg;

			obj3_msg.setAddress("/object3");
			auto center = object3Finder.getCenter(0);
			obj3_msg.addFloatArg(center.x);
			obj3_msg.addFloatArg(center.y);

			sender.sendMessage(obj3_msg, false);
		}
		else {
			ofxOscMessage obj3_msg;

			obj3_msg.setAddress("/!object3");

			sender.sendMessage(obj3_msg, false);
		}
    }
}

void ofApp::draw() {
    ofSetColor(255);
    cam.draw(0, 0);
    
    ofSetLineWidth(2);
    
	gloveFinder.draw();
	object1Finder.draw();
	object2Finder.draw();
	object3Finder.draw();

	if (gloveFinder.size() > 0) {
		auto center = gloveFinder.getCenter(0);
		ofSetColor(255);
		ofDrawCircle(center.x, center.y, 10);
	}

	if (object1Finder.size() > 0) {
		auto center = object1Finder.getCenter(0);
		ofSetColor(255);
		ofDrawCircle(center.x, center.y, 10);
	}

	if (object2Finder.size() > 0) {
		auto center = object2Finder.getCenter(0);
		ofSetColor(255);
		ofDrawCircle(center.x, center.y, 10);
	}

	if (object3Finder.size() > 0) {
		auto center = object3Finder.getCenter(0);
		ofSetColor(255);
		ofDrawCircle(center.x, center.y, 10);
	}

    gui.draw();
    
    ofTranslate(8, 75);
    ofFill();
    ofSetColor(0);	
    ofDrawRectangle(-3, -3 + 66, 64+6, 64+6);
	ofDrawBitmapString("Press: 1", 72, -3 + 66 + 33);
	
    ofSetColor(gloveColor);
    ofDrawRectangle(0, 0 + 66, 64, 64);

	ofSetColor(0);
	ofDrawRectangle(-3, -3 + 66 * 2, 64 + 6, 64 + 6);
	ofDrawBitmapString("Press: 2", 72, -3 + (66 * 2) + 33);
	ofSetColor(object1Color);
	ofDrawRectangle(0, 0 + 66 * 2, 64, 64);

	ofSetColor(0);
	ofDrawRectangle(-3, -3 + 66 * 3, 64 + 6, 64 + 6);
	ofDrawBitmapString("Press: 3", 72, -3 + (66 * 3) + 33);
	ofSetColor(object2Color);
	ofDrawRectangle(0, 0 + 66 * 3, 64, 64);

	ofSetColor(0);
	ofDrawRectangle(-3, -3 + 66 * 4, 64 + 6, 64 + 6);
	ofDrawBitmapString("Press: 4", 72, -3 + (66 * 4) + 33);
	ofSetColor(object3Color);
	ofDrawRectangle(0, 0 + 66 * 4, 64, 64);
}

void ofApp::mouseMoved(int x, int y) {
	mX = x;
	mY = y;
}

void ofApp::keyPressed(int key) {
	if (key == '1') {
		gloveColor = cam.getPixels().getColor(mX, mY);
	}

	if (key == '2') {
		object1Color = cam.getPixels().getColor(mX, mY);
	}

	if (key == '3') {
		object2Color = cam.getPixels().getColor(mX, mY);
	}

	if (key == '4') {
		object3Color = cam.getPixels().getColor(mX, mY);
	}
}
