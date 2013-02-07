
#include <windows.h>		// naglowek dla windowsa
#include <stdio.h>			//standardowe wejscie/wyjscie
#include <gl\gl.h>			//  OpenGL32 
#include <gl\glu.h>			//  GLu32 
#include <gl\glaux.h>		// Glaux Library



//#include <cstdio.h>

HDC			hDC=NULL;		// kontekst graficzny
HGLRC		hRC=NULL;		// kontekst renderowania
HWND		hWnd=NULL;		// uchwyt okna
HINSTANCE	hInstance;		// instancja aplikacji

bool	keys[256];			// stan klawiszy
bool	active=TRUE;		// flaga zminimalizowania, domyslnie na true
bool	fullscreen=TRUE;	// tryb pelnoekranowy, domyslnie na true

bool light;					// swiatlo wloczzone/wyloczonoe
bool lp;

GLUquadricObj *quadratic;							//miejsce przechowywania kwadryk

GLfloat LightAmbient[]={0.5f,0.5f,0.5f,1.0f};		//wartosc swiatla otaczajacego
GLfloat LightDiffuse[]={2.0f,2.0f,2.0f,1.0f};		//wartosc swiatla rozproszonego
GLfloat LightPosition[]={10.0f,10.0f,10.0f,1.0f};	//pozycja swiatla

GLfloat rtri;				//kat obrotu smigla wzgledem z
GLfloat rtquad;				//kat obrotu szescianu i smigiel wzgledem y

GLfloat zspeed;				// predkosc obracania z
GLfloat yspeed;				// predkosc obracania y
GLfloat z=-11;				// pozycja startowa z

GLuint texture[3];			//miejsce na  tekstury
GLuint loop;				//zmienna petel

LRESULT	CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);	// deklaracja WndProc

AUX_RGBImageRec *LoadBMP(char *Filename)				//laduje bitmape z pliku
{
	FILE *File=NULL;									//uchwyt pliku
	if (!Filename)										//upewnij sie ze podano nazwe pliku
	{
		return NULL;									//jezeli nie to zwroc NULL
	}
	File=fopen(Filename,"r");							//sprawdz czy plik istnieje
	if (File)											//czy plik istnieje
	{
		fclose(File);									//zamknij plik
		return auxDIBImageLoad(Filename);				//zaladuj bitmape i zwroc wskaznik
	}
	return NULL;										//jezeli nie udalo sie zaladowac pliku, zwroc null
}

int LoadGLTextures()									//laduj bitmape i przeksztalc ja w teksture
{
	int Status=FALSE;									//status tworzenia tekstury false-nie zaladowano lub nie zbudowano textury
	AUX_RGBImageRec *TextureImage[3];					//rekord dla naszego obrazka (zaiera wysokosc, szerokosc oraz dane bitmapy)
														//tworzy miejsce przechowywania tekstur
	memset(TextureImage,0,sizeof(void *)*3);			//wyzeruj wskaznik-wyczysc obrazek zeby upewnic sie ze jest pusta
	if ((TextureImage[0]=LoadBMP("mur.bmp"))&&(TextureImage[1]=LoadBMP("smiglo.bmp"))&&(TextureImage[2]=LoadBMP("trawa.bmp")))			//zaladuj obrazek i przeksztalcenie jej w teksture
	{
		Status=TRUE;

		glGenTextures(4,&texture[0]);					//tworzymy 2 obiekt tekstury ktora jest przecowywana w &texture[0]

		for (loop=0; loop<2;loop++)						//petla przez wszystkie tekstury
		{
		  glBindTexture(GL_TEXTURE_2D,texture[loop]);						//powiozanie z obiektem tekstury
		  glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);	//filtrowanie liniowe (dopasowanie rozmiaru)
		  glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
		  //tworzenie tekstury
		  glTexImage2D(GL_TEXTURE_2D,0,3,TextureImage[loop]->sizeX,TextureImage[loop]->sizeY,0,GL_RGB,GL_UNSIGNED_BYTE,TextureImage[loop]->data);

		}

//tworzenie mipmape
		  glBindTexture(GL_TEXTURE_2D,texture[2]);
		  glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);	//filtrowanie liniowe (dopasowanie rozmiaru)
		  glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR_MIPMAP_NEAREST);	
		  
		  gluBuild2DMipmaps(GL_TEXTURE_2D, 3, TextureImage[2]->sizeX, TextureImage[2]->sizeY, GL_RGB, GL_UNSIGNED_BYTE, TextureImage[2]->data);
		
	}

	//for (loop=0;loop<2;loop++)
	//{
	//	if (TextureImage[loop])			//jesli tekstura istnieje
	//		{
	//			if (TextureImage[loop]->data)		//jesli istnieje jej obraz	
	//			{
	//				free(TextureImage[loop]->data);	//to zwalniamy go
	//			}
	//			free(TextureImage[loop]);	//i zwalniamy strukture tekstury
	//	}
	//}
	return Status;										//zwroc status operacji
}

GLvoid ReSizeGLScene(GLsizei width, GLsizei height)		// Resize And Initialize The GL Window
{
	if (height==0)										// zapobieganie dzielenia przez zero
	{
		height=1;										//poprzez ustawienie wysokosci  na 1
	}

	glViewport(0,0,width,height);						// zresetowanie pola widzenia

	glMatrixMode(GL_PROJECTION);						// wybor maicerzy projekcjii
	glLoadIdentity();									// zresetowanie macierzy
// Obliczenie perspektywy dla okna
	gluPerspective(45.0f,(GLfloat)width/(GLfloat)height,0.1f,100.0f);
	glMatrixMode(GL_MODELVIEW);							// wybor macierzy modeli
	glLoadIdentity();									// reset macierzy modeli
}

int InitGL(GLvoid)										// inicializacja elementow OPENGL
{
	if (!LoadGLTextures())								//zaladuj tekstury
	{
		return FALSE;									//jezeli tekstury nie zaladowano zwroc false
	}
	glEnable(GL_TEXTURE_2D);							//wlacz nakladanie tekstur
	glShadeModel(GL_SMOOTH);							// wloczenie gladkiego cieniowania (miesza kolory i wygladza swiatlo)
	glClearColor(0.0f, 0.0f, 0.0f, 0.5f);				// ustawienie koloru czyszczenia ekranu na czarny
	glClearDepth(1.0f);									// ustawienie bufora glebi
	glEnable(GL_DEPTH_TEST);							// wlaczenie testowania glebi
	glDepthFunc(GL_LEQUAL);								// ustawienie typu testowania
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);	// wlaczenie najlepszej perspektywy

	glLightfv(GL_LIGHT1,GL_AMBIENT,LightAmbient);		//ustaw swiatlo otaczajoce	
	glLightfv(GL_LIGHT1,GL_DIFFUSE,LightDiffuse);		//ustaw swiatlo rozproszone
	glLightfv(GL_LIGHT1,GL_POSITION,LightPosition);		//pozycja swiatla
	
	glEnable(GL_LIGHT1);								//WLACZ SWIATLO numer 1

	glEnable(GL_CULL_FACE);								//wlaczenie face-culling

	quadratic=gluNewQuadric();							//ustawia wskaznik na nowa kwadryke
	gluQuadricNormals(quadratic,GLU_SMOOTH);			//tworzy gladkie normalne
	gluQuadricNormals(quadratic,GLU_TRUE);				//tworzy wspolrzedne tekstury


	return TRUE;										// poprawna inicjalizacja
}

int DrawGLScene(GLvoid)									// funkcja rysujaca
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);	// czyszczenie ekranu
	glLoadIdentity();									// zresetowania macierzu modeli
	glTranslatef(0.0f,2.0f, z);							//przesun w glab o 10 jednostek	
	
	glRotatef(rtquad,0.0f,3.0f,0.0f);					//obroc smigiel wzgledem y
	glRotatef(rtri,0.0f,0.0f,9.0f);						//obrot smigiel wzgledem z
	glTranslatef(-0.5f,-2.0f,0.5f);						//0.5 przed slopem wiatrak

	glBindTexture(GL_TEXTURE_2D,texture[1]);            //tekstura dla smkigiel
	glBegin(GL_TRIANGLES);								// rysowanie przy uzyciu trojkotow
//tylna sciana1
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f);glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f);glVertex3f( 1.0f,0.0f, 0.0f);	
		glTexCoord2f(0.0f,0.0f);glVertex3f(0.0f,0.0f, 0.0f);	
//przednia sciana1
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w strone widza
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f(0.0f,0.0f, 0.3f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f( 1.0f,0.0f, 0.3f);					
//tylna sciana2
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(2.25f,1.0f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.5f, 0.25f, 0.0f);
//przednia sciana2
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w strone widza
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.5f, 0.25f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(2.25f,1.0f, 0.3f);	
//tylna sciana3
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f); glVertex3f(2.25f,2.5f, 0.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( 2.25f, 1.5f, 0.0f);
//przednia sciana3
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 2.25f, 1.5f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(2.25f,2.5f, 0.3f);					

//tylna sciana4
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f); glVertex3f(1.5f,3.75f, 0.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( 2.25f, 3.0f, 0.0f);
//przednia sciana4
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 2.25f, 3.0f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(1.5f,3.75f, 0.3f);					
//tylna sciana5		
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f); glVertex3f(0.0f,4.0f, 0.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.0f, 4.0f, 0.0f);	
//przednia sciana5
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.0f, 4.0f, 0.3f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(0.0f,4.0f, 0.3f);					
//tylna sciana6
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f);glVertex3f(-1.25f,3.0f, 0.0f);
		glTexCoord2f(0.0f,0.0f); glVertex3f( -0.5f, 3.75f, 0.0f);
//przednia sciana6
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -0.5f, 3.75f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(-1.25f,3.0f, 0.3f);					
//tylna sciana7
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,0.0f); glVertex3f(-1.5f,1.5f, 0.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.5f, 2.5f, 0.0f);
//przednia sciana7
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f, 2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.5f, 2.5f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(-1.5f,1.5f, 0.3f);					
//tylna sciana8
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f,2.0f, 0.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(-0.5f,0.25f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.25f,1.0f, 0.0f);				
//przednia sciana8
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca w przod
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f,2.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.25f,1.0f, 0.3f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(-0.5f,0.25f, 0.3f);					
	glEnd();													// koniec rysowania trojkotow

	glBegin(GL_QUADS);											//poczatek rysowania czowrokotow
//smiglo1
	//koniec
		glNormal3f(0.0f,-1.0f,0.0f);							 //normalna wskazujaca W dol
		glTexCoord2f(1.0f,0.0f); glVertex3f( 1.0f, 0.0f, 0.0f);	 
		glTexCoord2f(1.0f,1.0f); glVertex3f(1.0f, 0.0f, 0.3f);	 
		glTexCoord2f(0.0f,1.0f); glVertex3f(0.0f,0.0f, 0.3f);	 
		glTexCoord2f(0.0f,0.0f); glVertex3f( 0.0f,0.0f, 0.0f);	 
	//prawa sciana
		glNormal3f(0.97f,0.25f,0.0f);							 
		glVertex3f( 0.5f, 2.0f, 0.0f);					
		glVertex3f(0.5f, 2.0f, 0.3f);					
		glVertex3f(1.0f,0.0f, 0.3f);					
		glVertex3f( 1.0f,0.0f, 0.0f);					
	//lewa sciana
		glNormal3f(- 0.97f,0.25f,0.0f);							 
		glVertex3f( 0.5f, 2.0f, 0.3f);					
		glVertex3f(0.5f, 2.0f, 0.0f);					
		glVertex3f(0.0f,0.0f, 0.0f);					
		glVertex3f( 0.0f,0.0f, 0.3f);					
//smiglo2
	//koniec
		glNormal3f(0.71f,0.71f,0.0f);							 //normalna wskazujaca W dol-prawo
		glTexCoord2f(1.0f,0.0f); glVertex3f( 2.25f, 1.0f, 0.0f);					
		glTexCoord2f(1.0f,1.0f); glVertex3f(2.25f, 1.0f, 0.3f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(1.5f,0.25f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.5f,0.25f, 0.0f);					
	//prawa sciana
		glNormal3f(-0.5f,-0.86f,0.0f);	
		glVertex3f( 2.25, 1.0f, 0.3f);					
		glVertex3f(2.25f, 1.0f, 0.0f);					
		glVertex3f(0.5f,2.0f, 0.0f);					
		glVertex3f( 0.5f,2.0f, 0.3f);					
	//lewa sciana
		glNormal3f(0.5f,0.86f,0.0f);
		glVertex3f( 1.5f, 0.25f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 0.5f,2.0f, 0.0f);				
		glVertex3f(1.5f, 0.25f, 0.0f);					
//smiglo3
	//koniec
		glNormal3f(1.0f,0.0f,0.0f);							 //normalna wskazujaca W prawo
		glTexCoord2f(1.0f,1.0f); glVertex3f( 2.25f, 2.5f, 0.0f);				
		glTexCoord2f(1.0f,0.0f); glVertex3f(2.25f, 2.5f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f(2.25f,1.5f, 0.3f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f( 2.25f,1.5f, 0.0f);					
	//prawa sciana
		glNormal3f(-0.26f,-0.97f,0.0f);
		glVertex3f( 2.25f, 2.5f, 0.3f);					
		glVertex3f(2.25f, 2.5f, 0.0f);					
		glVertex3f(0.5f,2.0f, 0.0f);					
		glVertex3f( 0.5f,2.0f, 0.3f);					
	//lewa sciana
		glNormal3f(-0.26f,0.97f,0.0f);
		glVertex3f( 2.25f, 1.5f, 0.0f);					
		glVertex3f( 2.25f,1.5f, 0.3f);					
		glVertex3f(0.5f, 2.0f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.0f);					
//smiglo4
	//koniec
		glNormal3f(0.71f,0.71f,0.0f);							 //normalna wskazujaca W gora-prawo
		glTexCoord2f(1.0f,1.0f); glVertex3f( 1.5f, 3.75f, 0.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(1.5f, 3.75f, 0.3f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(2.25f,3.0f, 0.3f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 2.25f,3.0f, 0.0f);					
	//prawa sciana
		glNormal3f(0.5f,-0.86f,0.0f);
		glVertex3f( 1.5f, 3.75f, 0.0f);					
		glVertex3f(0.5f, 2.0f, 0.0f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 1.5f,3.75f, 0.3f);					
	//lewa sciana
		glNormal3f(-0.5f,0.86f,0.0f);
		glVertex3f( 2.25f, 3.0f, 0.0f);					
		glVertex3f(2.25f, 3.0f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 0.5f,2.0f, 0.0f);					
//smiglo5
	//koniec
		glNormal3f(0.0f,1.0f,0.0f);							 //normalna wskazujaca w gore
		glTexCoord2f(1.0f,0.0f); glVertex3f( 1.0f, 4.0f, 0.3f);					
		glTexCoord2f(1.0f,1.0f); glVertex3f(1.0f, 4.0f, 0.0f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(0.0f,4.0f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 0.0f,4.0f, 0.3f);					
	//prawa sciana
		glNormal3f(-0.97f,-0.25f,0.0f);							 
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.0f, 4.0f, 0.3f);					
		glTexCoord2f(0.5f,1.0f); glVertex3f(0.0f, 4.0f, 0.0f);					
		glTexCoord2f(0.5f,1.0f); glVertex3f(0.5f,2.0f, 0.0f);					
		glTexCoord2f(0.5f,1.0f); glVertex3f( 0.5f,2.0f, 0.3f);					
	//lewa sciana
		glNormal3f(0.97f,-0.25f,0.0f);							 
		glVertex3f( 1.0f, 4.0f, 0.0f);					
		glVertex3f(1.0f, 4.0f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 0.5f, 2.0f, 0.0f);					
//smiglo6
	//koniec
		glNormal3f(-0.71f,0.71f,0.0f);							 //normalna wskazujaca W gora-lewo
		glTexCoord2f(1.0f,0.0f); glVertex3f( -0.5f, 3.75f, 0.3f);					
		glTexCoord2f(1.5f,1.0f); glVertex3f(-0.5f, 3.75f, 0.0f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(-1.25f,3.0f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.25f,3.0f, 0.3f);					
	//prawa sciana
		glNormal3f(0.5f,0.86f,0.0f);
		glVertex3f( -1.25f, 3.0f, 0.3f);				
		glVertex3f(-1.25f, 3.0f, 0.0f);					
		glVertex3f(0.5f,2.0f, 0.0f);					
		glVertex3f( 0.5f,2.0f, 0.3f);					
	//lewa sciana
		glNormal3f(-0.5f,-0.86f,0.0f);
		glVertex3f( -0.5f, 3.75f, 0.0f);				
		glVertex3f(-0.5f, 3.75f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 0.5f, 2.0f, 0.0f);					
//smiglo7
	//koniec
		glNormal3f(-1.0f,0.0f,0.0f);							 //normalna wskazujaca w lewo
		glTexCoord2f(1.0f,0.0f); glVertex3f( -1.5f, 2.5f, 0.3f);					
		glTexCoord2f(1.0f,1.0f); glVertex3f(-1.5f, 2.5f, 0.0f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(-1.5f,1.5f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.5f,1.5f, 0.3f);					
	//prawa sciana
		glNormal3f(0.5f,0.86f,0.0f);
		glVertex3f( -1.5f, 1.5f, 0.3f);					
		glVertex3f(-1.5f, 1.5f, 0.0f);					
		glVertex3f(0.5f,2.0f, 0.0f);					
		glVertex3f( 0.5f,2.0f, 0.3f);					
	//lewa sciana
		glNormal3f(0.5f,-0.86f,0.0f);
		glVertex3f( -1.5f, 2.5f, 0.0f);					
		glVertex3f(-1.5f, 2.5f, 0.3f);					
		glVertex3f(0.5f,2.0f, 0.3f);					
		glVertex3f( 0.5f, 2.0f, 0.0f);					
//smiglo8
	//koniec
		glNormal3f(-0.71f,-0.71f,0.0f);							 //normalna wskazujaca W dol-lewo
		glTexCoord2f(0.0f,0.0f); glVertex3f( -1.25f, 1.0f, 0.3f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f(-1.25f, 1.0f, 0.0f);					
		glTexCoord2f(1.0f,1.0f); glVertex3f(-0.5f,0.25f, 0.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f( -0.5f,0.25f, 0.3f);				
	//prawa sciana
		glNormal3f(0.5f,0.86f,0.0f);
		glVertex3f( -1.25f, 1.0f, 0.0f);	
		glVertex3f(-1.25f, 1.0f, 0.3f);		
		glVertex3f(0.5f,2.0f, 0.3f);		
		glVertex3f( 0.5f,2.0f, 0.0f);		
	//lewa sciana
		glNormal3f(0.5f,-0.86f,0.0f);
		glVertex3f( -0.5f, 0.25f, 0.3f);	
		glVertex3f(-0.5f, 0.25f, 0.0f);		
		glVertex3f(0.5f,2.0f, 0.0f);		
		glVertex3f( 0.5f, 2.0f, 0.3f);		
	glEnd();
	
	glLoadIdentity();							//zresetuj macierz modelowania
	glTranslatef(0.0f,0.0f,z);					//przesun wzgledem osi z (-11)
	glRotatef(rtquad,0.0f,3.0f,0.0f);			//obroc slup wzgledem y o kat rtquad

	glTranslatef(-0.5f,0.0f,0.5f);				//przesuniecie o -0.5 na osi x i 0.5 na z
	glBindTexture(GL_TEXTURE_2D,texture[0]);	//wybor tekstury mur
	glBegin(GL_QUADS);							//rysowanie czworokotow
//przedna sciana slupa
		glNormal3f(0.0f,0.0f,1.0f);								//normalna wskazujaca na widza
		glTexCoord2f(1.0f,1.0f); glVertex3f(1.0f, 2.0f, 0.0f);					
		glTexCoord2f(0.0f,1.0f); glVertex3f( 0.0f, 2.0f, 0.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( 0.0f,-4.0f, 0.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(1.0f,-4.0f, 0.0f);					
//lewa sciana slupa
		glNormal3f(-1.0f,0.0f,0.0f);							//normalna wskazujaca w lewo
		glTexCoord2f(0.0f,1.0f); glVertex3f( 0.0f, 2.0f, 0.0f);	
		glTexCoord2f(1.0f,1.0f);glVertex3f(0.0f, 2.0f, -1.0f);	
		glTexCoord2f(1.0f,0.0f);glVertex3f(0.0f,-4.0f, -1.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( 0.0f,-4.0f, 0.0f);	
//tylna sciana slupa
		glNormal3f(0.0f,0.0f,-1.0f);							//normalna wskazujaca w glab		
		glTexCoord2f(0.0f,1.0f); glVertex3f(0.0f, 2.0f, -1.0f);
		glTexCoord2f(1.0f,1.0f); glVertex3f( 1.0f, 2.0f, -1.0f);		
		glTexCoord2f(0.0f,0.0f); glVertex3f(1.0f,-4.0f, -1.0f);			
		glTexCoord2f(1.0f,0.0f);glVertex3f( 0.0f,-4.0f, -1.0f);			
//prawa sciana slupa
		glNormal3f(1.0f,0.0f,0.0f);								//normalna wskazujaca w prawo
		glTexCoord2f(1.0f,0.0f); glVertex3f(1.0f, 2.0f, -1.0f);	
		glTexCoord2f(0.0f,0.0f); glVertex3f( 1.0f, 2.0f, 0.0f);		
		glTexCoord2f(0.0f,1.0f); glVertex3f( 1.0f,-4.0f, 0.0f);		
//dol
		glTexCoord2f(1.0f,1.0f); glVertex3f( 1.0f, -4.0f, -1.0f);			
		glTexCoord2f(1.0f,0.0f);glVertex3f( 1.0f,-4.0f, 0.0f);				
		glTexCoord2f(0.0f,0.0f);glVertex3f(0.0f,-4.0f, 0.0f);				
		glTexCoord2f(0.0f,1.0f);glVertex3f(0.0f, -4.0f, -1.0f);				
//gora
		glTexCoord2f(1.0f,1.0f); glVertex3f( 1.0f, 2.0f, -1.0f);	
		glTexCoord2f(1.0f,0.0f); glVertex3f( 1.0f,2.0f, 0.0f);		
		glTexCoord2f(0.0f,0.0f); glVertex3f(0.0f,2.0f, 0.0f);		
		glTexCoord2f(0.0f,1.0f); glVertex3f(0.0f, 2.0f, -1.0f);		
	glEnd();													//koniec rysowania czworokotow

//dach, wykorzystanie cieniowanie Gourada (gladkie)
	glBegin(GL_TRIANGLES);
//tyl dachu
		glNormal3f(0.0f,0.0f,-1.0f);					//normalna wskazujaca w glab
		glColor3f( 0.823529, 0.411765, 0.117647);		//czekoladowy
		glVertex3f( 0.0f, 2.0f, -1.0f);					
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f(1.0f, 2.0f, -1.0f);					
		glColor3f( 0.752941, 0.752941, 0.752941);		//SREBRNY KOLOR
		glVertex3f(0.5f,3.0f, -0.5f);					
//prawa dachu
		glNormal3f(1.0f,0.0f,0.0f);						//normalna wskazujaca na prawo
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f( 1.0f, 2.0f, -1.0f);					
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f(1.0f, 2.0f, 0.0f);					
		glColor3f(0.752941, 0.752941, 0.752941);
		glVertex3f(0.5f,3.0f, -0.5f);					
//przod dachu
		glNormal3f(0.0f,0.0f,1.0f);						//normalna wskazujaca na widza
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f( 1.0f, 2.0f, 0.0f);					
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f(0.0f, 2.0f, 0.0f);					
		glColor3f(0.752941, 0.752941, 0.752941);
		glVertex3f(0.5f,3.0f, -0.5f);					
//lewa dachu
		glNormal3f(-1.0f,0.0f,0.0f);					//normalna wskazujaca na lewo
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f( 0.0f, 2.0f, 0.0f);					
		glColor3f(0.823529, 0.411765, 0.117647);
		glVertex3f(0.0f, 2.0f, -1.0f);					
		glColor3f(0.752941, 0.752941, 0.752941);
		glVertex3f(0.5f,3.0f, -0.5f);					

		glColor3f(1.0, 1.000000, 1.0);
	glEnd();
	glLoadIdentity();									//zresetuj macierz modelowania
	
	rtri+=zspeed;				//zwieksz predkosc obrotu smigiel
	rtquad+=yspeed;				//zwieksz predkosc obrotu smigiel i slupa

	glTranslatef(0.0f,-3.5f,z);		//przesuniecie wzgedem y o -3.5 i wartosc z(11)			
	glRotatef(90,1.0f,0.0f,0.0f);	//obroc cylinder o 90 deg wzgledem x

	gluCylinder(quadratic,0.8f,0.8f,0.5f,32,32);  //rysowanie cylindra
	
	glLoadIdentity();
	glTranslatef(0.0f,-3.5f,z);		
	glRotatef(90,-1.0f,0.0f,0.0f);						//obroc cylinder

	gluDisk(quadratic,0.0f,0.8f,32,32);					//rysowanie dysku przykrywajacego cylinder
	glLoadIdentity();									//zresetuj macierz modelowania
	glBindTexture(GL_TEXTURE_2D,texture[2]);			//wybor tekstury trawa
	glTranslatef(0.0f,0.0f,z);		
	
	glBegin(GL_QUADS);
		glNormal3f(0.0f,1.0f,.0f);												//normalna wskazujaca na widza
		glTexCoord2f(0.0f,1.0f); glVertex3f( -5.0f, -4.0f, 8.0f);				
		glTexCoord2f(1.0f,1.0f); glVertex3f(5.0f, -4.0f, 8.0f);					
		glTexCoord2f(1.0f,0.0f); glVertex3f(5.0f,-4.0f, -8.0f);					
		glTexCoord2f(0.0f,0.0f); glVertex3f( -5.0f,-4.0f,-8.0f);				
	glEnd();

	glLoadIdentity();									//zresetuj macierz modelowania

	return TRUE;										// zwrot statusu rysowania
}

GLvoid KillGLWindow(GLvoid)								// zamykanie okno
{
	if (fullscreen)										// jezeli tryb pelnoekranowy
	{
		ChangeDisplaySettings(NULL,0);					// przywrocenie orginalnej rozdzielczosci
		ShowCursor(TRUE);								// pokaz wskaznik myszy
	}

	if (hRC)											// jezeli mamy kontekst renderingu
	{
		if (!wglMakeCurrent(NULL,NULL))					// czy mozemy zwolnic kontekst renderingu
		{
			MessageBox(NULL,"Nie mo¿na zwolnic hDC lub hRC!","BLAD ZAMYKANIA",MB_OK | MB_ICONINFORMATION);
		}

		if (!wglDeleteContext(hRC))						// czy mozna zwolnic kontekst renderu
		{
			MessageBox(NULL,"Nie mozna usunoc kontekstu renderu","BLAD ZAMYKANIA",MB_OK | MB_ICONINFORMATION);
		}
		hRC=NULL;										// ustawiamy hRC na null
	}

	if (hDC && !ReleaseDC(hWnd,hDC))					// czy mozna zwolnic hDC
	{
		MessageBox(NULL,"Nie mozna zwolnic hWnd.","BLAD ZAMYKANIA",MB_OK | MB_ICONINFORMATION);
		hDC=NULL;										// ustawienie hDC na null
	}

	if (hWnd && !DestroyWindow(hWnd))					// czy mozna zwolnic uchwyt okna
	{
		MessageBox(NULL,"Nie mozna zwolnic hWnd.","BLAD ZAMYKANIA",MB_OK | MB_ICONINFORMATION);
		hWnd=NULL;										// ustawienie hWnd na null
	}

	if (!UnregisterClass("OpenGL",hInstance))			// czy mozemy wyrejestrowac klase okna
	{
		MessageBox(NULL,"Nie mozna wyrejestrowac klasy okna","BLAD ZAMYKANIA",MB_OK | MB_ICONINFORMATION);
		hInstance=NULL;									// ustawiamy instancje na NULL
	}
}
//tworzenie okna
 BOOL CreateGLWindow(char* title, int width, int height, int bits, bool fullscreenflag)
{
	GLuint		PixelFormat;			// przechowuje znaleziony format piksela
	WNDCLASS	wc;						// struktura klasy okna
	DWORD		dwExStyle;				// rozszerzony styl okna
	DWORD		dwStyle;				// normalny styl okna
	RECT		WindowRect;				// tu beda rozmiary okna
	WindowRect.left=(long)0;			// poczatek szerokosci od lewej ma 0
	WindowRect.right=(long)width;		// szerokosc bierzemy z parametru naszej funkcji
	WindowRect.top=(long)0;				// poczatek wysokosci zaczynamy od gory
	WindowRect.bottom=(long)height;		// wysokosc z parametru funkcji

	fullscreen=fullscreenflag;			// ustawiamy globalna zmienna

	hInstance			= GetModuleHandle(NULL);				// pobieramy instancje dla okna
	wc.style			= CS_HREDRAW | CS_VREDRAW | CS_OWNDC;	// ustawiamy odmalowanie
	wc.lpfnWndProc		= (WNDPROC) WndProc;					// WndProc obsluguje komunikaty
	wc.cbClsExtra		= 0;									
	wc.cbWndExtra		= 0;									
	wc.hInstance		= hInstance;							// ustawiamy instancje
	wc.hIcon			= LoadIcon(NULL, IDI_WINLOGO);			// domyslna ikona
	wc.hCursor			= LoadCursor(NULL, IDC_ARROW);			// kursor strzalka
	wc.hbrBackground	= NULL;									
	wc.lpszMenuName		= NULL;									
	wc.lpszClassName	= "OpenGL";								// nazwa klasy okna

	if (!RegisterClass(&wc))									// proba rejestracji klasy okna
	{
		MessageBox(NULL,"Nie udalo sie zarejestrowac klasy okna.","BLAD",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;											// zakonczenie i zwrot false
	}
	
	if (fullscreen)												// czy ma byc pelny ekran
	{
		DEVMODE dmScreenSettings;								// tryb karty graficznej
		memset(&dmScreenSettings,0,sizeof(dmScreenSettings));   // Wyczyœæ pamiêæ
		dmScreenSettings.dmSize = sizeof(dmScreenSettings);     // Ustaw rozmiar tej struktury
		dmScreenSettings.dmPelsWidth = width;					// Wybie¿ ¿¹dan¹ szerokoœæ
		dmScreenSettings.dmPelsHeight = height;					// Wybierz ¿¹dan¹ wysokoœæ 
		dmScreenSettings.dmBitsPerPel = bits;				    // Wybierz g³êbie kolorów 
		dmScreenSettings.dmFields=DM_BITSPERPEL|DM_PELSWIDTH|DM_PELSHEIGHT;

		 // Spróbuj ustawiæ pe³ny ekran. CDS_FULLSCREEN usuwa pasek start.
	if(ChangeDisplaySettings(&dmScreenSettings,CDS_FULLSCREEN)!=DISP_CHANGE_SUCCESSFUL)
	{ 
	 // Jeœli siê nie uda, przejdŸ do okna lub zakoñcz program 
		if(MessageBox(NULL,"Tryb graficzny nie jest obs³ugiwany przez twoj¹ kartê graf. Czy u¿yæ zamiast niego okna?","NeHe PL",MB_YESNO|MB_ICONEXCLAMATION)==IDYES)
		{ 
			fullscreen=FALSE;		// tryb okienkowy.  Fullscreen = FALSE
		}
		else
		{		// Uwaga o zamykaniu 
			MessageBox(NULL,"Program teraz siê zamknie.","B£¥D",MB_OK|MB_ICONSTOP);
			return FALSE;         // Zakoñcz i zwróæ false
		}
	}
	} 

	if (fullscreen)												// w trybie fullscreen
	{
		dwExStyle=WS_EX_APPWINDOW;								// rozszerzony styl okna
		dwStyle=WS_POPUP;										// styl okna
		ShowCursor(FALSE);										// ukryj kursor myszy
	}
	else
	{
		dwExStyle=WS_EX_APPWINDOW | WS_EX_WINDOWEDGE;			// rozszerzony styl okna
		dwStyle=WS_OVERLAPPEDWINDOW;							// styl okna
	}

	AdjustWindowRectEx(&WindowRect, dwStyle, FALSE, dwExStyle);		// dopasowanie okna do naszego stylu

	//tworzenie okna
	if( !(hWnd=CreateWindowEx( dwExStyle,       // Rozszerzony styl dla okna
		"OpenGL",								// Nazwa klasy 
		title,									// Tytu³ okna
		WS_CLIPSIBLINGS |				        // Wymagane style okna
		WS_CLIPCHILDREN |						// Wymagane style okn
		dwStyle,								// Wybrane style okna
		0, 0,									// Pozycja okna
		WindowRect.right-WindowRect.left,       // Szerokoœæ
		WindowRect.bottom-WindowRect.top,       // Wysokoœæ
		NULL,									// Nie u¿ywamy okna potomnego
		NULL,									// ¯adnego menu	
		hInstance,								// Instancja
		NULL)))									// Nie dawaj nic do WM_CREATE 
	{
		KillGLWindow();							//zrestuj tryb pelnoekranowy
		MessageBox(NULL,"Nie mozna stworzyc okna.","BLAD",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;								// zwroc FALSE
	}

	static PIXELFORMATDESCRIPTOR pfd =         // pfd mówi oknu co chcemy 
	{
		sizeof(PIXELFORMATDESCRIPTOR),         // Rozmiar opisu piksela
		1,								       // Numer wersji
		PFD_DRAW_TO_WINDOW |				   // Format musi obs³ugiwaæ okno
		PFD_SUPPORT_OPENGL |				   // Format musi obs³ugiwaæ OpenGL
		PFD_DOUBLEBUFFER,					   // Musi obs³ugiwaæ Podwójne buforowanie
		PFD_TYPE_RGBA,						   // i format RGBA
		bits,								   // Wybieramy g³êbie kolorów
		0, 0, 0, 0, 0, 0,					   // ignorujemy 
		0,									   // Bez bufora alpha 
		0,									   // Bit ignorujemy 
		0,									   // ignorujemy 
		0, 0, 0, 0,							   // ignorujemy 
		16,									   // 16 bitowy bufor Z 
		0,									   // ignorujemy 
		0,									   // ignorujemy 
		PFD_MAIN_PLANE,						   // G³ówna warstwa rysowania 
		0,									   // zarezerwowane 
		0, 0, 0								   // ignorujemy maski warstw 
	}; 
	
	if (!(hDC=GetDC(hWnd)))         // Mamy kontekst urz¹dzenia? 
	{
		KillGLWindow();             // Resetujemy ekran 
		MessageBox(NULL,"Nie mo¿na stworzyæ kontekstu urz¹dzenia.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;             // zwracamy false 
	} 
	if (!(PixelFormat=ChoosePixelFormat(hDC,&pfd)))         // Czy windows znajdzie taki format pixela? 
	{
		KillGLWindow();         // Resetujemy ekran 
		MessageBox(NULL,"Nie mo¿na znaleŸæ ¿¹danego formatu piksela.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;         // zwracamy false 
	} 
	if (!SetPixelFormat(hDC,PixelFormat,&pfd))         // Czy mo¿emy ustawiæ taki format 
	{
		KillGLWindow();         // Resetujemy ekran 
		MessageBox(NULL,"Nie mo¿na ustawiæ ¿¹danego formatu piksela.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;         // zwracamy false 
	} 
	if (!(hRC=wglCreateContext(hDC)))         // Czy mo¿emy pobraæ hRC 
	{
		KillGLWindow();         // Resetujemy ekran 
		MessageBox(NULL,"Nie mo¿na stworzyæ kontekstu renderowania.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;         // zwracamy false 
	} 

	if (!wglMakeCurrent(hDC,hRC))         // Czy mo¿emy aktywowaæ kontekst renderowania?
	{
		KillGLWindow();         // Resetujemy ekran 
		MessageBox(NULL,"Nie mo¿na aktywowaæ kontekstu renderowania.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;         // zwracamy false 
	} 

	ShowWindow(hWnd,SW_SHOW);       // Pokazujemy okno
	SetForegroundWindow(hWnd);      // Ustawiamy wy¿szy priorytet
	SetFocus(hWnd);					// Dzia³anie klawiatury skierowujemy na okno
	ReSizeGLScene(width, height);   // Ustawiamy perspektywê 

	if (!InitGL())         // Czy GL zanicjowa³ siê ? 
	{
		KillGLWindow();         // Resetujemy ekran 
		MessageBox(NULL,"Inicjacja niepomyœlna.","B£¥D",MB_OK|MB_ICONEXCLAMATION);
		return FALSE;         // zwracamy false 
	} 

	return TRUE;									// sukces
}

LRESULT CALLBACK WndProc(	HWND	hWnd,			// uchwyt do okna
							UINT	uMsg,			// wiadomosc dla okna
							WPARAM	wParam,			// dodatkowe informacje wiadomosci
							LPARAM	lParam)			// dodatkowe informacje wiadomosci
{
	switch (uMsg)									// sprawdz komunikat okna
	{
		case WM_ACTIVATE:							// czy to wiadomsc aktywowania
		{
			if (!HIWORD(wParam))					// czy program jest aktywowany
			{
				active=TRUE;						// program jest aktywny
			}
			else
			{
				active=FALSE;						// program nie jest aktywny
			}

			return 0;								// powrot do petli wiadomosc
		}

		case WM_SYSCOMMAND:							// czy to komenda systemowa
		{
			switch (wParam)							// sprawdzenei typu
			{
				case SC_SCREENSAVE:					// zgaszacz ekranu czce sie wlaczyc
				case SC_MONITORPOWER:				// Monitor chce sie wylaczyc
				return 0;							// anulowanie wygaszacza itp
			}
			break;									// koniec
		}

		case WM_CLOSE:								//czy to rozkaz zamkniecja
		{
			PostQuitMessage(0);						// wyslij wiadomsc zamkniecja
			return 0;								// skocz dalej
		}

		case WM_KEYDOWN:							// czy klawisz zostal wcisniety
		{
			keys[wParam] = TRUE;					// odpowiednie pole zostaje ustawione
			return 0;								// skocz dalej
		}

		case WM_KEYUP:								// CZY KLAWISZ ZOSTAL PUSZCZONY
			{
			keys[wParam] = FALSE;					// odpowiednie pole przyjmuje warosc false
			return 0;								// skok dalej
		}

		case WM_SIZE:								// czy okno sie zmienilo
		{
			ReSizeGLScene(LOWORD(lParam),HIWORD(lParam));  // zmieniamy scene 
			return 0;								// skocz dalej
		}
	}

	// reszta informacj idzie do WINDOWPROC
	return DefWindowProc(hWnd,uMsg,wParam,lParam);
}

int WINAPI WinMain(	HINSTANCE	hInstance,			// instancja
					HINSTANCE	hPrevInstance,		// poprzednia instancja
					LPSTR		lpCmdLine,			// parametr z linii komend
					int			nCmdShow)			// stan okna
{
	MSG		msg;									// strukltura przechowujaca komunikaty okna
	BOOL	done=FALSE;								// stan dzialania programu

	  // Zapytaj o tryb ekranu
	if (MessageBox(NULL,"Czy chcesz byæ w pe³nym ekranie?", "Start?",MB_YESNO|MB_ICONQUESTION)==IDNO)
	{
		fullscreen=FALSE;         // tryb okienkowym
	}
	// Stwórz okno OpenGL
	if (!CreateGLWindow("Wiatrak",640,480,16,fullscreen))
	{
		return 0;         // zakoñcz program
	} 

	while(!done)									// petla glowna programu
	{
		if (PeekMessage(&msg,NULL,0,0,PM_REMOVE))	// czy sa jakies wiadomosci
		{
			if (msg.message==WM_QUIT)				// czy otrzymano wiadomsc zamkniecja
			{
				done=TRUE;							// skoro tak, to done=TRUE
			}
			else									// nie otrzymano wiadomsci zamkniecia
			{
				TranslateMessage(&msg);				// wytlumacz wiadomosc
				DispatchMessage(&msg);				// wyslij ja
			}
		}
		else										//nie ma zadnych komunikatow
		{
			// przerwanie petli
			// Rysowanie sceny
			if (active)								// Program aktywny?
			{
				if (keys[VK_ESCAPE])				// ESC zostalo nacisniete
				{
					done=TRUE;						
				}
				else								// w innym wypadku aktualizacja ekranu
				{
					DrawGLScene();					// rysowanie sceny
					SwapBuffers(hDC);				// zamien buffory(ekrany)
				}
			}
			if (keys[VK_F1])						// F1 nacisniete
			{
				keys[VK_F1]=FALSE;					// ustawienie na false bo zostal uzyty
				KillGLWindow();						// zamknij okno
				fullscreen=!fullscreen;				// zamien pelny ekran
				// tworzenie nowego okna
				if (!CreateGLWindow("Wiatrak",640,480,16,fullscreen))
				{
					return 0;						// zamnij jezeli wystapil blad
				}
			}
				
				if (keys['L'] && !lp)				//obsluga oswietlenia
				{
					lp=TRUE;
					light=!light;
					if (!light)
					{
						glDisable(GL_LIGHTING);
					}
					else
					{
						glEnable(GL_LIGHTING);
					}
				}
				if (!keys['L'])
				{
					lp=FALSE;
				}
				
				if (keys[VK_PRIOR])						//PAGE UP zostalo nacisniete
				{
					z-=0.02f;							//przesuniece obiektu w glab ekranu
				}

				if (keys[VK_NEXT])						//PAGE DOWN zostalo nacisniete
				{
					z+=0.02f;							//przesuniece obiektu w kierunku widza
				}

				if (keys[VK_UP])         // Strza³ka w górê zosta³a wciœniêta? 
				{
					zspeed-=0.01f;         // zspeed zwieksza obroty smigiel zgodnie z zegarem
				}
				if (keys[VK_DOWN])         // Strza³ka w dó³ zosta³a wciœniêta?
				{
					zspeed+=0.01f;         // Jeœli tak, zwiêksz zspeed odwrotnie do zegara
				}
				if (keys[VK_RIGHT])         // Strza³ka w prawo zosta³a wciœniêta?
				{
				yspeed+=0.01f;				//  zwiêksz yspeed, obraca obiekt w prawo
				}
				if (keys[VK_LEFT])         // Strza³ka w lewo zosta³a wciœniêta?
				{
				yspeed-=0.01f;	         // obraca obiekt w lewo
				} 

				if (keys[VK_F1])						// czy F1 wcisniete?
				{
					keys[VK_F1]=FALSE;					// ustaw go na false, bo zostal uzyty
					KillGLWindow();						// zamknij okno
					fullscreen=!fullscreen;				// Tzamien pelny ekran
					// stworz nowe okno
					if (!CreateGLWindow("Wiatrak",640,480,16,fullscreen))
					{
						return 0;						// wystapil blad
					}
				}
			}
		
	}

	// zamykanie
	KillGLWindow();									// zabij okno
	return (msg.wParam);							// koniec program
}
