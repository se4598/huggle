//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

#include "hugglequeueitemlabel.h"
#include "ui_hugglequeueitemlabel.h"

int HuggleQueueItemLabel::Count = 0;

HuggleQueueItemLabel::HuggleQueueItemLabel(QWidget *parent) : QFrame(parent), ui(new Ui::HuggleQueueItemLabel)
{
    ui->setupUi(this);
}

HuggleQueueItemLabel::~HuggleQueueItemLabel()
{
    delete ui;
}

void HuggleQueueItemLabel::SetName(QString name)
{
    ui->label_2->setText(name);
}

QString HuggleQueueItemLabel::GetName()
{
    return ui->label_2->text();
}

void HuggleQueueItemLabel::Process()
{
    HuggleQueueItemLabel::Count--;
    Core::ProcessEdit(this->page);
    this->hide();
    this->close();
}

void HuggleQueueItemLabel::on_label_2_linkActivated(const QString &link)
{
    QApplication::exit();
}

void HuggleQueueItemLabel::mousePressEvent(QMouseEvent *event)
{
    if (event->button() == Qt::LeftButton)
    {
        this->Process();
    }
}