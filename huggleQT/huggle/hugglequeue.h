//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

#ifndef HUGGLEQUEUE_H
#define HUGGLEQUEUE_H

#include <QDockWidget>
#include "hugglequeuefilter.h"

namespace Ui {
class HuggleQueue;
}

class HuggleQueueFilter;

class HuggleQueue : public QDockWidget
{
    Q_OBJECT
    
public:
    explicit HuggleQueue(QWidget *parent = 0);
    ~HuggleQueue();
    
private:
    Ui::HuggleQueue *ui;
    HuggleQueueFilter *CurrentFilter;
};

#endif // HUGGLEQUEUE_H