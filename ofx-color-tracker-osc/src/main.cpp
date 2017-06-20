#include "ofApp.h"

int main(int argc, char *argv[]) {
	ofSetupOpenGL(640, 480, OF_WINDOW);
	ofApp *app = new ofApp();
	app->arguments = vector<string>(argv, argv + argc);
	ofRunApp(new ofApp());	
}
